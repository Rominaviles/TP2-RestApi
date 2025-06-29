using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ProApprovalStep
{
    public interface IApprovalStepCommand
    {
        Task<ProjectApprovalStep?> GetProjectApprovalSteByUserAndIdAsync(Guid stepId, int userId);
        Task<bool> UpdateStepStatusAsync(ProjectApprovalStep step, int newStatus);
        Task<bool> UpdateProposalStatusAsync(Guid proposalId, int newStatus);
        Task <bool>CreateProjectApprovalStepAsync(ProjectApprovalStep step);
        Task<bool> CreateProposalWithSteps(ProjectProposal proposal, List<ProjectApprovalStep> steps);
    }
}
