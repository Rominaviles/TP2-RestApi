using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Interfaces.AppStatus
{
    public interface IApprovalRuleService
    {
        Task<bool> ApprovalRuleExistsAsync(int ruleId);
        Task<List<ApprovalRule>> GetAllApprovalRulesAsync();
        Task<List<ApprovalRule>> GetAllApprovalRuleByAreaAndType(int? areaId, int? typeId, decimal amount); 

    }

}
