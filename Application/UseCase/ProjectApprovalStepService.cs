using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.ProApprovalStep;
using Domain.Entities;
using Interfaces.ProApprovalStep;
using Domain.Enums;
using System.Linq;
using Application.Interfaces.ProProposal;



namespace Application.Services
{
    public class ProjectApprovalStepService : IApprovalStepService
    {
        private readonly IApprovalStepCommand approvalStepCommand;
        private readonly IApprovalStepQuery approvalStepQuery;
        private readonly IProjectProposalQuery projectProposalQuery;
        private readonly IProjectProposalCommand projectProposalCommand;

        public ProjectApprovalStepService(IApprovalStepCommand command, IApprovalStepQuery query, IProjectProposalQuery projectProposalQuery, IProjectProposalCommand projectProposalCommand)
        {
            this.approvalStepCommand = command;
            this.approvalStepQuery = query;
            this.projectProposalQuery = projectProposalQuery;
            this.projectProposalCommand = projectProposalCommand;
        }

        public async Task<ProjectApprovalStep> GetProjectApprovalStepByIdAsync(long id)
        {
            return await approvalStepQuery.GetProjectApprovalStepByIdAsync(id);
        }

        public async Task<List<ProjectApprovalStep>> GetAllApprovalStepByProposalIdAsync(Guid proposalId)
        {
            return await approvalStepQuery.GetAllApprovalStepByProposalIdAsync(proposalId);
        }

        public async Task<List<ProjectApprovalStep>> GetPendingApprovalStepByUserIdAsync(int userId)
        {
            return await approvalStepQuery.GetPendingApprovalStepByUserIdAsync(userId);
        }

        public async Task<bool> AreAllStepsApprovedAsync(Guid proposalId)
        {
            return await approvalStepQuery.AreAllStepsApprovedAsync(proposalId);
        }

        public async Task<bool> IsStepRejectedAsync(Guid stepId, int userId)
        {
            return await approvalStepQuery.IsStepRejectedAsync(stepId, userId);
        }

        public async Task<bool> ApproveStepAsync(Guid proposalId, int userId, int status, string observations = " ")
        {
            if (status < 1 || status > 4)
                throw new ArgumentOutOfRangeException(nameof(status), "El estado no es válido.");

            var allSteps = await approvalStepQuery.GetAllApprovalStepByProposalIdAsync(proposalId);

            var step = allSteps
                .Where(s =>
                    (s.Status == 1 || s.Status == 4) &&
                    s.ApproverRole != null &&
                    s.ApproverRole.User.Any(u => u.Id == userId))
                .OrderBy(s => s.StepOrder)
                .FirstOrDefault();

            if (step == null)
                throw new UnauthorizedAccessException("No hay pasos pendientes para este usuario o el usuario no tiene permisos.");

            if (step.Status == 2 || step.Status == 3)
                throw new InvalidOperationException("No se puede modificar un paso ya aprobado o rechazado.");

            var previousSteps = allSteps.Where(p => p.StepOrder < step.StepOrder);
            if (previousSteps.Any(p => p.Status != 2))
                throw new ArgumentException("No puede aprobar este paso hasta que se aprueben todos los pasos anteriores.");

            // Actualizar el paso
            step.ApproverUserId = userId;
            step.Status = status;
            step.Observations = observations;
            step.DecisionDate = DateTime.UtcNow;

            await approvalStepCommand.UpdateStepStatusAsync(step, status);

            // Si fue Rechazado u Observado, el proyecto se actualiza
            if (status == 3)
                return await approvalStepCommand.UpdateProposalStatusAsync(step.ProjectProposalId, 3);

            if (status == 4)
                return await approvalStepCommand.UpdateProposalStatusAsync(step.ProjectProposalId, 4);

            // Si fue Aprobado, solo actualizar el proyecto si TODOS los pasos están aprobados
            if (status == 2)
            {
                bool allApproved = allSteps.All(s => s.Status == 2 || s.Id == step.Id);
                if (allApproved)
                    return await approvalStepCommand.UpdateProposalStatusAsync(step.ProjectProposalId, 2);
            }

            // No actualizar el estado del proyecto si no corresponde
            return true;
        }


        public async Task<bool> ObserveAsync(ProjectApprovalStep step, string comment)
        {
            step.Status = 4; // Observado
            step.Observations = comment;
            step.DecisionDate = DateTime.UtcNow;

            var proposal = await projectProposalQuery.GetProjectProposalByIdAsync(step.ProjectProposalId);
            proposal.Status = 4; // Observado
            await projectProposalCommand.UpdateProjectProposalAsync(proposal);

            return await approvalStepCommand.UpdateStepStatusAsync(step, step.Status);
        }

    }
}
