using Application.Interfaces.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using System.Dynamic;
using Application.Modal.Request;



namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserQuery userQuery;
        private readonly IUserCommand userCommand;

        public UserService(IUserQuery query, IUserCommand command)
        {
            this.userQuery = query;
            this.userCommand = command;
        }
        public async Task<bool> UserExistAsync(string email)
        {
            return await userCommand.UserExistAsync(email);
        }

        public async Task<UserRequest> GetUserByIdAsync(int userId)
        {
            if (userId == 0)
            {
                return new UserRequest
                {
                    Id = 0,
                    Name = "No asignado",
                    Email = "-",
                    RoleId = 0
                };
            }

            var user = await userQuery.GetUserByIdAsync(userId);

            if (user == null)
            {
                return new UserRequest
                {
                    Id = userId,
                    Name = "Usuario no encontrado",
                    Email = "-",
                    RoleId = 0
                };
            }

            return new UserRequest
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                RoleId = user.Role
            };
        }

        public async Task<UserRequest> GetUserByEmailAsync(string email)
        {
            var user = await userQuery.GetUserByEmailAsync(email);
            if (user == null) return null;

            return new UserRequest
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                RoleId = user.Role
            };

        }
    }
}
