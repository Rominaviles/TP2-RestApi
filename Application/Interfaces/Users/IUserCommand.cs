using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Users
{
    public interface IUserCommand
    {
        Task<bool> UserExistAsync(string email);
    }
}
