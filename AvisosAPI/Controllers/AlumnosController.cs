using AvisosAPI.Models.DTOs;
using AvisosAPI.Models.Entities;
using AvisosAPI.Services;
using AvisosAPI.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AvisosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumnosController : ControllerBase
    {
        private readonly AlumnosService service;
        private readonly AvisosService avisosService;
        private readonly IValidator<AgregarAlumnoDTO> agregarValidator;
        private readonly IValidator<EditarAlumnoDTO> editarValidator;

        public AlumnosController(AlumnosService service, AvisosService avisosService, IValidator<AgregarAlumnoDTO> agregarValidator, IValidator<EditarAlumnoDTO> editarValidator)
        {
            this.service = service;
            this.avisosService = avisosService;
            this.agregarValidator = agregarValidator;
            this.editarValidator = editarValidator;
            this.agregarValidator = agregarValidator;
        }

        [HttpPost]
        [Authorize(Roles = "Maestro")]
        public IActionResult AgregarAlumno(AgregarAlumnoDTO dto)
        {
            try
            {
                int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
                var validation = agregarValidator.Validate(dto);
                if (!validation.IsValid)
                {
                    return BadRequest(validation.Errors.Select(e => e.ErrorMessage));
                }
                service.AgregarAlumno(dto, idUsuario);
                return Ok();
            }
            catch(Exception ex)
            {
                //return BadRequest(ex.Message);
                return BadRequest(new List<string> { ex.Message });
            }
        }

        [HttpGet("{idAlumno}")]
        [Authorize(Roles = "Maestro")]
        public IActionResult VerDetallesAlumno(int idAlumno)
        {
            try
            {
                int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
                var clase = service.VerDetallesAlumno(idAlumno, idUsuario);
                return Ok(clase);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = "Maestro")]
        public IActionResult EditarAlumno(EditarAlumnoDTO dto)
        {
            try
            {
                int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
                var validation = editarValidator.Validate(dto);
                if (!validation.IsValid)
                {
                    return BadRequest(validation.Errors.Select(e => e.ErrorMessage));
                }
                service.EditarAlumno(dto, idUsuario);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{idAlumno}")]
        [Authorize(Roles = "Maestro")]
        public IActionResult EliminarAlumno(int idAlumno)
        {
            try
            {
                int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
                service.EliminarAlumno(idAlumno, idUsuario);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
