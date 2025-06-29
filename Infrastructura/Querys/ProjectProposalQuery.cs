using Application.Interfaces.ProProposal;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Querys
{
    public class ProjectProposalQuery : IProjectProposalQuery
    {
        private readonly ApprovalProjectDB context;

        public ProjectProposalQuery(ApprovalProjectDB context)
        {
            this.context = context;
        }

        public async Task<bool> ProjectProposalExistAsync(Guid proposalId)
        {
            return await context.ProjectProposal
                 .AnyAsync(p =>
                     p.Id.Equals(proposalId)
                 );
        }
        public async Task<ProjectProposal?> GetProjectProposalByIdAsync(Guid proposalId)
        {
            return await context.ProjectProposal
                .Include(p => p.ProjectApprovalSteps)
                    .ThenInclude(s => s.ApprovalStatus)
                .Include(p => p.ProjectApprovalSteps)
                    .ThenInclude(s => s.User)
                .Include(p => p.ProjectApprovalSteps)
                    .ThenInclude(s => s.ApproverRole)
                .Include(p => p.Areas)
                .Include(p => p.ProjectType)
                .Include(p => p.User)
                    .ThenInclude(s => s.ApproverRole)
                .FirstOrDefaultAsync(p => p.Id == proposalId);
        }
        public async Task<List<ProjectProposal>> GetAllProjectProposalsByUserIdAsync(int userId)
        {
            return await context.ProjectProposal
                .Include(p => p.ProjectApprovalSteps)
                .Include(p => p.Areas)
                .Include(p => p.ProjectType)
                .Include(p => p.ApprovalStatus)
                .Where(p => p.CreateBy == userId)
                .ToListAsync();
        }

        public async Task<int> GetProjectProposalStatusAsync(Guid proposalId)
        {
            var proposal = await context.ProjectProposal
                .Where(p => p.Id == proposalId)
                .Select(p => p.Status)
                .FirstOrDefaultAsync();

            return proposal;
        }

        public async Task<bool> TitleExistAsync(string title, Guid? excludeId = null)
        {
            return await context.ProjectProposal
                .AnyAsync(p => p.Title == title && (excludeId == null || p.Id != excludeId));
        }

        public async Task<List<ProjectProposal>> GetFilteredProjectsAsync(string? title, int? status, int? applicant, int? approvalUser)
        {
            var query = context.ProjectProposal
                .Include(p => p.Areas)
                .Include(p => p.ProjectType)
                .Include(p => p.ApprovalStatus)
                .Include(p => p.ProjectApprovalSteps)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(p => p.Title.Contains(title));

            if (status.HasValue)
                query = query.Where(p => p.Status == status.Value);

            if (applicant.HasValue)
                query = query.Where(p => p.CreateBy == applicant.Value);

            int? approverRoleId = null;

            if (approvalUser.HasValue)
            {
                approverRoleId = await context.User
                    .Where(u => u.Id == approvalUser.Value)
                    .Select(u => (int?)u.Role)
                    .FirstOrDefaultAsync();

                if (approverRoleId == null)
                    return new List<ProjectProposal>();

                query = query.Where(p =>
                    p.ProjectApprovalSteps.Any(s =>
                        s.Status == 1 &&
                        s.ApproverRoleId == approverRoleId.Value
                    )
                );
            }

            var lista = await query.ToListAsync();

            if (approvalUser.HasValue && approverRoleId.HasValue)
            {
                lista = lista.Where(p =>
                {
                    var primerPendiente = p.ProjectApprovalSteps
                        .Where(s => s.Status == 1)
                        .OrderBy(s => s.StepOrder)
                        .FirstOrDefault();

                    return primerPendiente != null &&
                           primerPendiente.ApproverRoleId == approverRoleId.Value;
                }).ToList();
            }

            return lista;
        }



    }
}