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
        Task<bool> ProjectTypeExistsAsync(int typeId);
        Task<bool> AreaExistsAsync(int areaId);
        Task<bool> UserExistsAsync(int userId);
        Task<List<User>> GetAllUsersAsync();
    }
}
