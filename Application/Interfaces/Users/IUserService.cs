using Application.Modal.Request;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Users
{
    public interface IUserService
    {
        Task<bool> UserExistAsync(string email);
        Task<UserRequest> GetUserByIdAsync(int userId);
        Task<UserRequest> GetUserByEmailAsync(string email);

    }
}
