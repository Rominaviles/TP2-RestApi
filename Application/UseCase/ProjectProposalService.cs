using Application.Exceptions;
using Application.Helpers;
using Application.Interfaces.AppRule;
using Application.Interfaces.Information;
using Application.Interfaces.ProApprovalStep;
using Application.Interfaces.ProProposal;
using Application.Interfaces.Users;
using Application.Mapper;
using Application.Modal.Request;
using Application.Modal.Response;
using Domain.Entities;
using Domain.Enums;
using Interfaces.ProProposal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace Application.Services
{
    public class ProjectProposalService : IProjectProposalService
    {
        //Injectar el Command y Query
        private readonly IProjectProposalCommand _projectProposalCommand;
        private readonly IProjectProposalQuery _projectProposalQuery;
        private readonly IApprovalRuleQuery _approvalRuleQuery;
        private readonly IApprovalStepCommand _approvalStepCommand;
        private readonly IApprovalStepQuery _approvalStepQuery;
        private readonly IUserQuery _userQuery;
        private readonly IInformationQuery _informationQuery;


        public ProjectProposalService(IProjectProposalCommand command, IProjectProposalQuery query, IApprovalRuleQuery ruleQuery, IApprovalStepCommand stepCommand, IApprovalStepQuery stepQuery, IUserQuery userQuery, IInformationQuery informationQuery)
        {
            this._projectProposalCommand = command;
            this._projectProposalQuery = query;
            this._approvalRuleQuery = ruleQuery;
            this._approvalStepCommand = stepCommand;
            this._approvalStepQuery = stepQuery;
            this._userQuery = userQuery;
            this._informationQuery = informationQuery;
        }


        public async Task<ProjectProposalResponse> CreateProjectProposal(ProjectProposalRequest dto, int createdByUserId)
        {
            // Validación del título
            dto.title = dto.title?.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(dto.title))
                throw new BadRequestException("El título es obligatorio.");

            // Validación del usuario
            var user = await _userQuery.GetUserByIdAsync(createdByUserId);
            if (user == null)
                throw new BadRequestException("El usuario que crea el proyecto no existe.");

            // Validación de duplicados por título
            if (await _projectProposalQuery.TitleExistAsync(dto.title, null))
                throw new ConflictException("Ya existe una propuesta con ese título.");

            // Validación de descripción
            if (string.IsNullOrWhiteSpace(dto.description))
                throw new BadRequestException("La descripción es obligatoria.");

            // Validación del monto
            if (dto.amount <= 0)
                throw new BadRequestException("El monto debe ser mayor a cero.");

            // Validación de la duración
            if (dto.duration <= 0)
                throw new BadRequestException("La duración debe ser mayor a cero.");

            // Validación del área existente
            bool areaExists = await _informationQuery.AreaExistsAsync(dto.area);
            if (!areaExists)
                throw new BadRequestException("El área indicada no existe.");

            // Validación del tipo de proyecto existente
            bool typeExists = await _informationQuery.ProjectTypeExistsAsync(dto.type);
            if (!typeExists)
                throw new BadRequestException("El tipo indicado no existe.");

            // Crear la entidad de la propuesta
            var proposal = new ProjectProposal
            {
                Id = Guid.NewGuid(),
                Title = dto.title,
                Description = dto.description,
                Area = dto.area,
                Type = dto.type,
                EstimatedAmount = dto.amount,
                EstimatedDuration = dto.duration,
                CreateAt = DateTime.UtcNow,
                CreateBy = createdByUserId,
                Status = 1 // Pendiente
            };

            // Obtener las reglas de aprobación aplicables de manera eficiente
            var applicableRules = await _approvalRuleQuery.GetAllApprovalRuleByAreaAndType(dto.area, dto.type, dto.amount);

            // Seleccionar las reglas con mayor prioridad
            var selectedRules = applicableRules
                .GroupBy(p => p.StepOrder)
                .Select(group =>
                    group.OrderByDescending(r =>
                        (r.Area.HasValue ? 1 : 0) +
                        (r.Type.HasValue ? 1 : 0) +
                        (r.MaxAmount > 0 ? 1 : 0))
                    .First()
                )
                .OrderBy(r => r.StepOrder)
                .ToList();

            // Crear los pasos de aprobación según las reglas seleccionadas
            var steps = new List<ProjectApprovalStep>();

            foreach (var rule in selectedRules.OrderBy(r => r.StepOrder))
            {
                var step = new ProjectApprovalStep
                {
                    ProjectProposalId = proposal.Id,
                    ApproverRoleId = rule.ApproverRoleId,
                    StepOrder = rule.StepOrder, // 👈 Usar el StepOrder real de la regla
                    Status = 1,
                    ApproverUserId = null
                };
                steps.Add(step);
            }

            // Guardar propuesta y pasos en transacción
            var success = await _approvalStepCommand.CreateProposalWithSteps(proposal, steps);
            if (!success)
                throw new InvalidOperationException("No se pudo guardar la propuesta.");

            // Obtener la propuesta recién creada para retorno
            var proposalCompleted = await _projectProposalQuery.GetProjectProposalByIdAsync(proposal.Id);
            if (proposalCompleted == null)
                throw new NotFoundException("No se pudo obtener la propuesta recién creada.");

            return ProjectMapper.ProjectProposalMapper.DetailDTO(proposalCompleted);
        }


        public async Task<ProjectProposalResponse> GetProjectProposalDetailAsync(Guid proposalId)
        {
            var proposal = await _projectProposalQuery.GetProjectProposalByIdAsync(proposalId);
            if (proposal == null)
                throw new NotFoundException("La propuesta de proyecto no fue encontrada.");

            return ProjectMapper.ProjectProposalMapper.DetailDTO(proposal);
        }


        public async Task<ProjectProposalResponse> UpdateProjectProposalAsync(Guid proposalId, ProjectProposalUpdateRequest dto)
        {
            var proposal = await _projectProposalQuery.GetProjectProposalByIdAsync(proposalId);
            if (proposal == null)
                throw new NotFoundException("La propuesta de proyecto no fue encontrada.");

            if (proposal.Status != 4)
                throw new InvalidOperationException("Solo se pueden modificar proyectos en estado Observado.");

            if (string.IsNullOrWhiteSpace(dto.title))
                throw new BadRequestException("El título es obligatorio.");

            if (string.IsNullOrWhiteSpace(dto.description))
                throw new BadRequestException("La descripción es obligatoria.");

            if (dto.duration <= 0)
                throw new BadRequestException("La duración estimada debe ser mayor a cero.");

            string normalizedTitle = dto.title.Trim().ToLower();
            bool titleExists = await _projectProposalQuery.TitleExistAsync(normalizedTitle, proposalId);
            if (titleExists)
                throw new ConflictException("Ya existe una propuesta con ese título.");

            proposal.Title = dto.title;
            proposal.Description = dto.description;
            proposal.EstimatedDuration = dto.duration;

            var updated = await _projectProposalCommand.UpdateProjectProposalAsync(proposal);
            if (!updated)
                throw new InvalidOperationException("Error al actualizar la propuesta.");

            var updatedProposal = await _projectProposalQuery.GetProjectProposalByIdAsync(proposalId);
            return ProjectMapper.ProjectProposalMapper.DetailDTO(updatedProposal);
        }


        public async Task<List<FilteredResopnse>> GetFilteredProjectsAsync(string? title, int? status, int? applicant, int? approvalUser)
        {
            if (status.HasValue && (status < 1 || status > 4))
                throw new BadRequestException("Parámetro de consulta del estado inválido");

            if (applicant.HasValue && applicant < 1)
                throw new BadRequestException("Parámetro de consulta de aplicante inválido.");

            if (approvalUser.HasValue && approvalUser < 1)
                throw new BadRequestException("Parámetro de consulta del aprobador inválido.");

            var projects = await _projectProposalQuery.GetFilteredProjectsAsync(title, status, applicant, approvalUser);

            if (projects == null || !projects.Any())
                return new List<FilteredResopnse>();

            return projects.Select(p =>
            {
                var dto = ProjectMapper.FilteredDTO(p);

                var estadoCalculado = ProjectStatusHelper.CalcularEstadoProyecto(p.ProjectApprovalSteps.ToList());

                dto.Status = estadoCalculado.ToString();

                return dto;
            }).ToList();
        }



    }
}

