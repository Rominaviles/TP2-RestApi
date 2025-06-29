using Application.Interfaces.Information;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Querys
{
    public class InformationQuery : IInformationQuery
    {
        private readonly ApprovalProjectDB _context;
        public InformationQuery(ApprovalProjectDB context)
        {
            _context = context;
        }
        public async Task<List<Area>> GetAllAreasAsync()
        {
            return await _context.Area.ToListAsync();
        }

        public async Task<List<ProjectType>> GetAllProjectTypesAsync()
        {
            return await _context.ProjectType.ToListAsync();
        }

        public async Task<List<ApproverRole>> GetAllRolesAsync()
        {
            return await _context.ApproverRole.ToListAsync();
        }

        public async Task<List<ApprovalStatus>> GetAllApprovalStatusesAsync()
        {
            return await _context.ApprovalStatus.ToListAsync();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.User
                .Include(u => u.ApproverRole)
                .ToListAsync();
        }
    }
}
