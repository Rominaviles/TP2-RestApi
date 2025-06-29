using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Users;
using Domain.Entities;
using System.Reflection.Metadata.Ecma335;


namespace Infrastructure.Querys
{
    public class UserQuery : IUserQuery
    {
        private readonly ApprovalProjectDB context;

        public UserQuery(ApprovalProjectDB context)
        {
            this.context = context;
        }
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await context.User
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await context.User
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetFirstUserByIdAsync(int roleId)
        {
            return await context.User
                .FirstOrDefaultAsync(u => u.Role == roleId);
        }
        public async Task<List<User>> GetUsersByRoleIdAsync(int roleId)
        {
            return await context.User
                .Where(u => u.Role == roleId)
                .ToListAsync();
        }

    }
}
