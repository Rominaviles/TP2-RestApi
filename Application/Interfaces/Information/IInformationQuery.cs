using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Information
{
    public interface IInformationQuery
    {
        Task<List<Area>> GetAllAreasAsync();
        Task<List<ProjectType>> GetAllProjectTypesAsync();
        Task<List<ApproverRole>> GetAllRolesAsync();
        Task<List<ApprovalStatus>> GetAllApprovalStatusesAsync();
        Task<List<User>> GetAllUsersAsync();
    }
}
