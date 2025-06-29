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

        // Obtener todas las áreas (solo si es necesario)
        public async Task<List<Area>> GetAllAreasAsync()
        {
            return await _context.Area.AsNoTracking().ToListAsync();
        }

        // Verificar si un área existe
        public async Task<bool> AreaExistsAsync(int areaId)
        {
            return await _context.Area.AsNoTracking()
                .AnyAsync(a => a.Id == areaId);
        }

        // Obtener todos los tipos de proyectos 
        public async Task<List<ProjectType>> GetAllProjectTypesAsync()
        {
            return await _context.ProjectType.AsNoTracking().ToListAsync();
        }

        // Verificar si un tipo de proyecto existe
        public async Task<bool> ProjectTypeExistsAsync(int typeId)
        {
            return await _context.ProjectType.AsNoTracking()
                .AnyAsync(t => t.Id == typeId);
        }

        // Obtener todos los roles de aprobador
        public async Task<List<ApproverRole>> GetAllRolesAsync()
        {
            return await _context.ApproverRole.AsNoTracking().ToListAsync();
        }

        // Obtener todos los estados de aprobación
        public async Task<List<ApprovalStatus>> GetAllApprovalStatusesAsync()
        {
            return await _context.ApprovalStatus.AsNoTracking().ToListAsync();
        }

        // Verificar si un usuario existe
        public async Task<bool> UserExistsAsync(int userId)
        {
            return await _context.User.AsNoTracking()
                .AnyAsync(u => u.Id == userId);
        }

        // Obtener todos los usuarios
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.User
                .Include(u => u.ApproverRole)
                .AsNoTracking()
                .ToListAsync();
        }
    }

}
