using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;

namespace Interfaces.ProApprovalStep
{
    public interface IApprovalStepService
    {
        Task<ProjectApprovalStep> GetProjectApprovalStepByIdAsync(long stepId);
        Task<List<ProjectApprovalStep>> GetAllApprovalStepByProposalIdAsync(Guid proposalId);
        Task<List<ProjectApprovalStep>> GetPendingApprovalStepByUserIdAsync(int userId);
        Task<bool> AreAllStepsApprovedAsync(Guid proposalId);
        Task<bool> IsStepRejectedAsync(Guid stepdId, int userId);
        Task<bool> ApproveStepAsync(Guid proposalId, int userId, int status, string observations = " ");
        Task<bool> ObserveAsync(ProjectApprovalStep step, string comment);
    }
}
