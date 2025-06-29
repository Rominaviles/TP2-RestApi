using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Application.Modal.Response;
using Application.Modal.Request;
using Application.Services;

namespace Interfaces.ProProposal
{
    public interface IProjectProposalService
    {
        Task<ProjectProposalResponse> CreateProjectProposal(ProjectProposalRequest dto, int createdByUserId);
        Task<ProjectProposalResponse> GetProjectProposalDetailAsync(Guid proposalId);
        Task<ProjectProposalResponse> UpdateProjectProposalAsync(Guid proposalId, ProjectProposalUpdateRequest dto);
        Task<List<FilteredResopnse>> GetFilteredProjectsAsync(string? title, int? status, int? applicant, int? approvalUser);
    }
}