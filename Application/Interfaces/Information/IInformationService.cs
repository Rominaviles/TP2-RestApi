using Application.Modal.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Information
{
    public interface IInformationService
    {
        Task<List<GenericResponse>> GetAllAreasAsync();
        Task<List<GenericResponse>> GetAllProjectTypesAsync();
        Task<List<GenericResponse>> GetAllRolesAsync();
        Task<List<GenericResponse>> GetAllApprovalStatusesAsync();
        Task<List<UserResponseDTO>> GetAllUsersAsync();
    }
}
