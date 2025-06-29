using Application.Modal.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public static class AreaMapper
    {
        public static GenericResponse ToDTO(Area entity)
        {
            return new GenericResponse
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}
