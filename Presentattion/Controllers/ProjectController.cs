using Application.Interfaces;
using Application.Mapper;
using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Interfaces.ProProposal;
using Application.Modal.Request;
using System.ComponentModel.DataAnnotations;
using Application.Modal.Response;
using Application.Services;
using Domain.Enums;
using Interfaces.ProApprovalStep;
using Azure;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectProposalService _service;
        private readonly IApprovalStepService _serviceApproval;
        public ProjectController(IProjectProposalService service, IApprovalStepService serviceApproval)
        {
            _service = service;
            _serviceApproval = serviceApproval;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<FilteredResopnse>), 200)]
        [ProducesResponseType(typeof(ApiErrorException), 400)]
        public async Task<IActionResult> GetFilteredProjects([FromQuery] string? title, [FromQuery] int? status, [FromQuery] int? applicant, [FromQuery] int? approvalUser)
        {
            try
            {
                var result = await _service.GetFilteredProjectsAsync(title, status, applicant, approvalUser);
                return Ok(result);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpPost]
        [ProducesResponseType(typeof(ProjectProposalResponse), 201)]
        [ProducesResponseType(typeof(ApiErrorException), 400)]
        [ProducesResponseType(typeof(ApiErrorException), 409)]
        public async Task<IActionResult> CreateProject([FromBody] ProjectProposalRequest dto)
        {
            try
            {
                var result = await _service.CreateProjectProposal(dto, dto.user);
                return CreatedAtAction(nameof(GetProjectById), new { result.id }, result);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new ApiErrorException { Message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new ApiErrorException { Message = ex.Message });
            }
            catch (ConflictException ex)
            {
                return Conflict(new ApiErrorException { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiErrorException { Message = ex.Message });
            }
        }


        [HttpPost("{id}/decision")]
        [ProducesResponseType(typeof(ProjectProposalResponse), 200)]
        [ProducesResponseType(typeof(ApiErrorException), 400)]
        [ProducesResponseType(typeof(ApiErrorException), 404)]
        [ProducesResponseType(typeof(ApiErrorException), 409)]
        public async Task<IActionResult> DecideStep(Guid id, [FromBody] DecisionStepRequest dto)
        {
            try
            {
                // Validación del ID del proyecto para asegurarse de que sea un GUID válido
                if (id == Guid.Empty)
                {
                    return BadRequest(new { message = "El ID del proyecto no es un GUID válido." });
                }

                // Validación del usuario
                if (dto.User <= 0 || dto.User > 6)
                {
                    return BadRequest(new { message = "ID de usuario inválido." });
                }

                // Validar que el status esté dentro del rango correcto
                if (dto.Status < 1 || dto.Status > 4)
                {
                    return BadRequest(new { message = "El estado no es válido." });
                }

                var result = await _serviceApproval.ApproveStepAsync(id, dto.User, dto.Status, dto.Observation);

                if (!result)
                {
                    return Conflict(new { message = "El estado del proyecto no se pudo actualizar correctamente." });
                }

                var updatedProject = await _service.GetProjectProposalDetailAsync(id);
                return Ok(updatedProject);
            }
            catch (ConflictException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error inesperado.", error = ex.Message });
            }
        }




        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProjectProposalResponse), 200)]
        [ProducesResponseType(typeof(ApiErrorException), 400)]
        [ProducesResponseType(typeof(ApiErrorException), 404)]
        [ProducesResponseType(typeof(ApiErrorException), 409)]
        [ProducesResponseType(typeof(ApiErrorException), 500)]
        public async Task<IActionResult> UpdateProject(Guid id, [FromBody] ProjectProposalUpdateRequest dto)
        {
            try
            {
                var updated = await _service.UpdateProjectProposalAsync(id, dto);
                return Ok(updated);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message }); 
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { message = ex.Message }); 
            }
            catch (ConflictException ex)
            {
                return Conflict(new { message = ex.Message }); 
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Fallback general ante cualquier error inesperado
                return StatusCode(500, new { message = "Ocurrió un error inesperado.", error = ex.Message });
            }
        }



        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProjectProposalResponse), 200)]
        [ProducesResponseType(typeof(ApiErrorException), 400)]
        [ProducesResponseType(typeof(ApiErrorException), 404)]
        public async Task<IActionResult> GetProjectById(Guid id)
        {
            try
            {
                var result = await _service.GetProjectProposalDetailAsync(id);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
