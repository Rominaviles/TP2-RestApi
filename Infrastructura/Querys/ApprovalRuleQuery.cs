using Application.Interfaces.AppRule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Querys
{
    public class ApprovalRuleQuery : IApprovalRuleQuery
    {
        private readonly ApprovalProjectDB context;

        public ApprovalRuleQuery(ApprovalProjectDB context)
        {
            this.context = context;
        }

        public async Task<bool> ApprovalRuleExistsAsync(int ruleId)
        {
            return await context.ApprovalRule
                .AnyAsync(p => p.Id.Equals(ruleId));
        }
        public  async Task<List<ApprovalRule>> GetAllApprovalRulesAsync()
        {
            return await context.ApprovalRule
                .Include(p => p.Areas)
                .Include(p => p.ProjectType)
                .Include(p => p.ApproverRole)
                .ToListAsync();
        }
        public async Task<List<ApprovalRule>> GetAllApprovalRuleByAreaAndType(int? areaId, int? typeId, decimal amount)
        {
            var rules = await context.ApprovalRule
             .Where(p =>
                 p.MinAmount <= amount &&
                 (p.MaxAmount == 0 || amount <= p.MaxAmount) &&
                 (p.Area == null || p.Area == areaId) &&
                 (p.Type == null || p.Type == typeId)
             )
             .Include(p => p.ApproverRole)
             .ToListAsync();

            var selectedRules = rules
                .GroupBy(r => r.StepOrder)
                .Select(group =>
                    group
                    .OrderByDescending(r =>
                        (r.Type != null ? 1 : 0) +
                        (r.Area != null ? 1 : 0) +
                        (r.MinAmount > 0 ? 1 : 0) +
                        (r.MaxAmount > 0 ? 1 : 0)
                    )
                    .First()
                )
                .ToList();

            return selectedRules;
        }

    }
}
