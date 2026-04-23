using AvisosAPI.Models.DTOs;
using AvisosAPI.Models.Entities;
using AvisosAPI.Repositories;
using AvisosAPI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AvisosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvisosController : ControllerBase
    {
        private readonly AvisosService service;
        private readonly IValidator<AgregarAvisoPersonalDTO> agregarPersonalValidator;
        private readonly IValidator<AgregarAvisoGeneralDTO> agregarGeneralValidator;

        public AvisosController(AvisosService service, IValidator<AgregarAvisoPersonalDTO> agregarPersonalValidator, IValidator<AgregarAvisoGeneralDTO> agregarGeneralValidator)
        {
            this.service = service;
            this.agregarPersonalValidator = agregarPersonalValidator;
            this.agregarGeneralValidator = agregarGeneralValidator;
        }
        [HttpGet("alumnos/{idAlumno}")]
        public IActionResult VerAvisosPersonalesAlumno(int idAlumno)
        {
            try
            {
                var avisos = service.VerAvisosPersonalesAlumno(idAlumno);
                return Ok(avisos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{idAviso}/alumno")]
        public IActionResult VerDetallesAvisoAlumno(int idAviso)
        {
            try
            {
                int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
                var detalles = service.VerDetallesAvisoPersonalAlumno(idAviso, idUsuario);
                return Ok(detalles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{idAviso}/maestro")]
        public IActionResult VerDetallesAvisoMaestro(int idAviso)
        {
            try
            {
                int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
                var detalles = service.VerDetallesAvisoPersonalMaestro(idAviso, idUsuario);
                return Ok(detalles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("generales/alumno/{idAlumno}")]
        public IActionResult VerAvisosGeneralesAlumno(int idAlumno)
        {
            try
            {
                int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
                var avisos = service.VerAvisosGeneralesAlumno(idAlumno, idUsuario);
                return Ok(avisos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("generales/maestro/{idMaestro}")]
        public IActionResult VerAvisosGeneralesMaestro(int idMaestro)
        {
            try
            {
                int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
                var avisos = service.VerAvisosGeneralesMaestro(idMaestro, idUsuario);
                return Ok(avisos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("generales")] 
        public IActionResult AgregarAvisoGeneral(AgregarAvisoGeneralDTO dto)
        {
            try
            {
                int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
                var validation = agregarGeneralValidator.Validate(dto);
                if (!validation.IsValid)
                {
                    return BadRequest(validation.Errors.Select(e => e.ErrorMessage));
                }
                service.AgregarAvisoGeneral(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("generales/{idAviso}")]
        public IActionResult EliminarAvisoGeneral(int idAviso)
        {
            try
            {
                int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
                service.EliminarAvisoGeneral(idAviso, idUsuario);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("alumno")]
        public IActionResult AgregarAvisoPersonal(AgregarAvisoPersonalDTO dto)
        {
            try
            {
                int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
                var validation = agregarPersonalValidator.Validate(dto);
                if (!validation.IsValid)
                {
                    return BadRequest(validation.Errors.Select(e => e.ErrorMessage));
                }
                service.AgregarAvisoPersonal(dto, idUsuario);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{idAviso}")]
        public IActionResult EliminarAvisoPersonal(int idAviso)
        {
            try
            {
                int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
                service.EliminarAvisoPersonal(idAviso, idUsuario);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("notificaciones/alumno/{idAlumno}")]
        public IActionResult RecibirNotificacionesAlumno(int idAlumno)
        {
            try
            {
                int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
                var notificaciones = service.VerNotificacionesAlumno(idAlumno, idUsuario);
                return Ok(notificaciones);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("notificaciones/maestro/{idMaestro}")]
        public IActionResult RecibirNotificacionesMaestro(int idMaestro)
        {
            try
            {
                int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
                var notificaciones = service.VerNotificacionesMaestro(idMaestro, idUsuario);
                return Ok(notificaciones);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
