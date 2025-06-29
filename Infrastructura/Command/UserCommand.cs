using Application.Interfaces.Users;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Command
{
    public class UserCommand : IUserCommand
    {
        private readonly ApprovalProjectDB context;

        public UserCommand(ApprovalProjectDB context)
        {
            this.context = context;
        }

        public async Task<bool> UserExistAsync(string email)
        {
            return await context.User
                .AnyAsync(u => u.Email == email);

        }
    }
}
