using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.ProApprovalStep
{
    public interface IApprovalStepQuery
    {
        Task<ProjectApprovalStep> GetProjectApprovalStepByIdAsync(long stepId);
        Task<List<ProjectApprovalStep>> GetAllApprovalStepByProposalIdAsync(Guid proposalId);
        Task<List<ProjectApprovalStep>> GetPendingApprovalStepByUserIdAsync(int userId);
        Task<bool> AreAllStepsApprovedAsync(Guid proposalId);
        Task<bool> IsStepRejectedAsync(Guid stepId, int userId);
    }
}
