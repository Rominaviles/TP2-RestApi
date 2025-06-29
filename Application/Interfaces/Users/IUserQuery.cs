using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Users
{
    public interface IUserQuery
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetFirstUserByIdAsync(int roleId);
        Task<List<User>> GetUsersByRoleIdAsync(int roleId);
    }
}
