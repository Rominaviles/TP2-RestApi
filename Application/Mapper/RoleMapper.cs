using Application.Modal.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public static class RoleMapper
    {
        public static GenericResponse ToDTO(ApproverRole entity)
        {
            return new GenericResponse
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}
