using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using Application.Interfaces.Information;
using Application.Exceptions;
using Application.Mapper;
using System.Data;
using Application.Modal.Response;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Controllers
{
    [Route("api/")]
    [ApiController]
    public class InformationController : ControllerBase
    {
        private readonly IInformationService _infoService;

        public InformationController(IInformationService infoService)
        {
            _infoService = infoService;
        }

        /// <summary>
        /// Listado de Áreas
        /// </summary>
        [HttpGet("Area")]
        [ProducesResponseType(typeof(IEnumerable<GenericResponse>), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ApiErrorException), StatusCodes.Status404NotFound)]
        //[ProducesResponseType(typeof(ApiErrorException), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<GenericResponse>>> GetAreas()
        {
            return await TryCatch(async () => await _infoService.GetAllAreasAsync());
        }

        /// <summary>
        /// Listado de tipos de proyectos
        /// </summary>
        [HttpGet("ProjectType")]
        [ProducesResponseType(typeof(IEnumerable<GenericResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GenericResponse>>> GetProjectTypes()
        {
            return await TryCatch(async () => await _infoService.GetAllProjectTypesAsync());
      
        }

        /// <summary>
        /// Listado de roles de usuario
        /// </summary>
        [HttpGet("Role")]
        [ProducesResponseType(typeof(IEnumerable<GenericResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GenericResponse>>> GetRoles()
        {
            return await TryCatch(async () => await _infoService.GetAllRolesAsync());
      
        }

        /// <summary>
        /// Listado de estados para una solicitud de proyecto y pasos de aprobación
        /// </summary>
        [HttpGet("ApprovalStatus")]
        [ProducesResponseType(typeof(IEnumerable<GenericResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GenericResponse>>> GetStatuses()
        {

            return await TryCatch(async () => await _infoService.GetAllApprovalStatusesAsync());

        }

        /// <summary>
        /// Listado de usuarios
        /// </summary>
        [HttpGet("User")]
        [ProducesResponseType(typeof(IEnumerable<UserResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserResponseDTO>>> GetUsers()
        {
            return await TryCatch(async () => await _infoService.GetAllUsersAsync());

        }

        private async Task<ActionResult<IEnumerable<T>>> TryCatch<T>(Func<Task<List<T>>> func)
        { 
                return Ok(await func());
        }
    }
}
