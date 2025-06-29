using Application.Modal.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public static class UserMapper
    {
        public static UserResponseDTO ToDTO(User entity)
        {
            return new UserResponseDTO
            {
                id = entity.Id,
                name = entity.Name,
                email = entity.Email,
                role = new GenericResponse
                {
                    Id = entity.ApproverRole.Id,
                    Name = entity.ApproverRole.Name
                }
            };
        }
    }
}
