using Application.Modal.Response;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public static class ApprovalStatusMapper
    {
        public static GenericResponse ToDTO(ApprovalStatus entity)
        {
           // var name = ((ApprovalStatusEnum)entity.Id).ToString();
            return new GenericResponse
            {
                Id = entity.Id,
                Name = entity.Name
            };       
        }
    }
}
