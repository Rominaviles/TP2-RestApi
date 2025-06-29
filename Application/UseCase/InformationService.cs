using Application.Interfaces.Information;
using Application.Mapper;
using Application.Modal.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class InformationService : IInformationService
    {
        private readonly IInformationQuery _informationQuery;
        public InformationService(IInformationQuery informationQuery)
        {
            _informationQuery = informationQuery;
        }
        public async Task<List<GenericResponse>> GetAllAreasAsync()
        {
            var areas = await _informationQuery.GetAllAreasAsync();
            return areas.Select(AreaMapper.ToDTO).ToList();
        }

        public async Task<List<GenericResponse>> GetAllProjectTypesAsync()
        {
            var types = await _informationQuery.GetAllProjectTypesAsync();
            return types.Select(ProjectTypeMapper.ToDTO).ToList();
        }

        public async Task<List<GenericResponse>> GetAllRolesAsync()
        {
            var roles = await _informationQuery.GetAllRolesAsync();
            return roles.Select(RoleMapper.ToDTO).ToList();
        }

        public async Task<List<GenericResponse>> GetAllApprovalStatusesAsync()
        {
            var status = await _informationQuery.GetAllApprovalStatusesAsync();
            return status.Select(ApprovalStatusMapper.ToDTO).ToList();
        }

        public async Task<List<UserResponseDTO>> GetAllUsersAsync()
        {
            var users = await _informationQuery.GetAllUsersAsync();
            return users.Select(UserMapper.ToDTO).ToList();
        }
    }
}
