using Application.Interfaces.AppRule;
using Interfaces.AppStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;


namespace Application.Services
{
    public class ApprovalRuleService : IApprovalRuleService
    {
        private readonly IApprovalRuleQuery approvalRuleQuery;
        public ApprovalRuleService(IApprovalRuleQuery query)
        {
            this.approvalRuleQuery = query;
        }
        public async Task<bool> ApprovalRuleExistsAsync(int ruleId)
        {
            return await approvalRuleQuery.ApprovalRuleExistsAsync(ruleId);
        }
        public async Task<List<ApprovalRule>> GetAllApprovalRulesAsync()
        {
            return await approvalRuleQuery.GetAllApprovalRulesAsync();
        }
        public async Task<List<ApprovalRule>> GetAllApprovalRuleByAreaAndType(int? areaId, int? typeId, decimal amount)
        {
            return await approvalRuleQuery.GetAllApprovalRuleByAreaAndType(areaId, typeId, amount);
        }

    }
}
