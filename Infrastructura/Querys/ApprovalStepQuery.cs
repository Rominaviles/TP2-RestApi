using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.ProApprovalStep;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Querys
{
    public class ApprovalStepQuery : IApprovalStepQuery 
    {
        private readonly ApprovalProjectDB context;

        public ApprovalStepQuery(ApprovalProjectDB context)
        {
            this.context = context;
        }

        public async Task<ProjectApprovalStep?> GetProjectApprovalStepByIdAsync(long stepId)
        {
            return await context.ProjectApprovalStep
                .Include(p => p.ProjectProposal)
                .Include(p => p.User)
                .Include(p => p.ApproverRole)
                .Include(p => p.ApprovalStatus)
                .FirstOrDefaultAsync(p => p.Id == stepId); 
        }
        public async Task<List<ProjectApprovalStep>> GetAllApprovalStepByProposalIdAsync(Guid proposalId)
        { 
            return await context.ProjectApprovalStep
                .Include(p => p.ApprovalStatus)
                .Include(p => p.ApproverRole)
                    .ThenInclude(r => r.User)
                .Where(p => p.ProjectProposalId == proposalId)
                .OrderBy(p => p.StepOrder)
                .ToListAsync();
        }

        public async Task<List<ProjectApprovalStep>> GetPendingApprovalStepByUserIdAsync(int userId)
        {
            var user = await context.User.FirstOrDefaultAsync(u => u.Id == userId);

            var steps = await context.ProjectApprovalStep
                .Include(p => p.ProjectProposal)
                    .ThenInclude(p => p.Areas)
                .Include(p => p.ProjectProposal)
                    .ThenInclude(p => p.ProjectType)
                .Where(p =>
                    p.ApproverRoleId == user.Role &&
                    (p.Status == 1 ||
                     (p.Status == 4 && p.ApproverUserId == userId)) &&
                    (p.ApproverUserId == null || p.ApproverUserId == userId))
                .ToListAsync();

            
            var validSteps = new List<ProjectApprovalStep>();

            foreach (var step in steps)
            {
                var allSteps = await context.ProjectApprovalStep
                    .Where(s => s.ProjectProposalId == step.ProjectProposalId)
                    .ToListAsync();

                var previousSteps = allSteps.Where(s => s.StepOrder < step.StepOrder).ToList();

                bool allPreviousApproved = previousSteps.All(p => p.Status == 2);

                if (allPreviousApproved)
                {
                    validSteps.Add(step);
                }
            }

            return validSteps;
        }

        public async Task<bool> AreAllStepsApprovedAsync(Guid proposalId)
        {
            return await context.ProjectApprovalStep
                .Where(p => p.ProjectProposalId == proposalId)
                .AllAsync(p => p.Status == 2);
                
        }

        public async Task<bool> IsStepRejectedAsync(Guid stepId, int userId)
        {
            var step = await context.ProjectApprovalStep
                .FirstOrDefaultAsync(p => p.Id.Equals(stepId)
                && p.ApproverUserId == userId);
            return step != null && step.Status == 3; 
        }

    }
}
