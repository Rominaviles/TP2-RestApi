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

namespace Infrastructure.Command
{
    public class ApprovalStepCommand : IApprovalStepCommand
    {
        private readonly ApprovalProjectDB context;

        public ApprovalStepCommand(ApprovalProjectDB context)
        {
            this.context = context;
        }

        public async Task<ProjectApprovalStep?> GetProjectApprovalSteByUserAndIdAsync(Guid stepId, int userId)
        {
            return await context.ProjectApprovalStep
                .Include (p => p.ProjectProposal)
                .Include (p => p.User)
                .Include(p => p.ApproverRole)
                .Include(p => p.ApprovalStatus)
                .FirstOrDefaultAsync(s => s.Id.Equals(stepId)
                && s.ApproverUserId == userId);
        }   
        public async Task<bool> UpdateStepStatusAsync(ProjectApprovalStep step, int newStatus)
        {
            step.Status = newStatus;
            context.ProjectApprovalStep.Update(step);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task <bool> UpdateProposalStatusAsync (Guid proposalId, int newStatus)
        {
            var proposal = await context.ProjectProposal
                .FirstOrDefaultAsync(p => p.Id == proposalId);
            if (proposal != null)
            {
                proposal.Status = newStatus;
                context.ProjectProposal.Update(proposal);
                return await context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<bool> CreateProjectApprovalStepAsync(ProjectApprovalStep step)
        {
            await context.ProjectApprovalStep.AddAsync(step);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CreateProposalWithSteps(ProjectProposal proposal, List<ProjectApprovalStep> steps)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await context.ProjectProposal.AddAsync(proposal);
                await context.ProjectApprovalStep.AddRangeAsync(steps);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }

}
