using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.AppRule
{
    public interface IApprovalRuleQuery
    {
        Task<bool> ApprovalRuleExistsAsync(int ruleId);
        Task<List<ApprovalRule>> GetAllApprovalRulesAsync();
        Task<List<ApprovalRule>> GetAllApprovalRuleByAreaAndType(int? areaId, int? typeId, decimal amount);

    }
}
