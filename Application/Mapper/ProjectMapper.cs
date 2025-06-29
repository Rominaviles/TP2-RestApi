using Application.Modal.Request;
using Application.Modal.Response;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Helpers;

namespace Application.Mapper
{
    public static class ProjectMapper
    {
        public static class ProjectProposalMapper
        {
            public static ProjectProposalResponse DetailDTO(ProjectProposal entity)
            {
                return new ProjectProposalResponse
                {
                    id = entity.Id,
                    title = entity.Title,
                    description = entity.Description,
                    amount = entity.EstimatedAmount,
                    duration = entity.EstimatedDuration,

                    area = entity.Areas != null
                        ? new GenericResponse
                        {
                            Id = entity.Areas.Id,
                            Name = entity.Areas.Name
                        }
                        : null,

                    status = new GenericResponse
                    {
                        Id = (int)ProjectStatusHelper.CalcularEstadoProyecto(entity.ProjectApprovalSteps?.ToList() ?? new List<ProjectApprovalStep>()),
                        Name = ProjectStatusHelper.CalcularEstadoProyecto(entity.ProjectApprovalSteps?.ToList() ?? new List<ProjectApprovalStep>()).ToString()  // Calculado automáticamente
                    },

                    type = entity.ProjectType != null
                        ? new GenericResponse
                        {
                            Id = entity.ProjectType.Id,
                            Name = entity.ProjectType.Name
                        }
                        : null,

                    user = entity.User != null
                        ? new UserResponseDTO
                        {
                            id = entity.User.Id,
                            name = entity.User.Name,
                            email = entity.User.Email,
                            role = entity.User.ApproverRole != null
                                ? new GenericResponse
                                {
                                    Id = entity.User.ApproverRole.Id,
                                    Name = entity.User.ApproverRole.Name
                                }
                                : null
                        }
                        : null,

                    steps = entity.ProjectApprovalSteps?
                        .Select(step => new ProjectApprovalStepResponse
                        {
                            stepOrder = step.StepOrder,
                            decisionDate = step.DecisionDate,
                            observations = step.Observations,

                            approverUser = step.ApproverUserId != null && step.User != null
                                ? new UserResponseDTO
                                {
                                    id = step.User.Id,
                                    name = step.User.Name,
                                    email = step.User.Email,
                                    role = step.User.ApproverRole != null
                                        ? new GenericResponse
                                        {
                                            Id = step.User.ApproverRole.Id,
                                            Name = step.User.ApproverRole.Name
                                        }
                                        : null
                                }
                                : null,

                            approverRole = step.ApproverRole != null
                                ? new GenericResponse
                                {
                                    Id = step.ApproverRole.Id,
                                    Name = step.ApproverRole.Name
                                }
                                : null,

                            status = new GenericResponse
                            {
                                Id = step.Status, // El estado ya está asignado correctamente al paso
                                Name = ((ApprovalStatusEnum)step.Status).ToString() // Calculamos el nombre basado en el valor de Status
                            }
                        }).ToList() ?? new List<ProjectApprovalStepResponse>()
                };
            }

        }

        public static FilteredResopnse FilteredDTO(ProjectProposal entity)
        {
            return new FilteredResopnse
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Amount = (double)entity.EstimatedAmount,
                Duration = entity.EstimatedDuration,
                Area = entity.Areas?.Name ?? "",
                Status = entity.ApprovalStatus?.Id.ToString() ?? "",
                Type = entity.ProjectType?.Name ?? ""
            };
        }

    }
}
