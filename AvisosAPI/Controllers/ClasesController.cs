using AvisosAPI.Models.DTOs;
using AvisosAPI.Models.Entities;
using AvisosAPI.Repositories;
using AvisosAPI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AvisosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClasesController : ControllerBase
    {
        private readonly ClasesService service;
        private readonly IValidator<AgregarClaseDTO> agregarValidator;
        private readonly IValidator<EditarClaseDTO> editarValidator;

        public ClasesController(ClasesService service, IValidator<AgregarClaseDTO> agregarValidator, IValidator<EditarClaseDTO> editarValidator)
        {
            this.service = service;
            this.agregarValidator = agregarValidator;
            this.editarValidator = editarValidator;
        }

        [HttpGet("{idMaestro}")]
        [Authorize(Roles = "Maestro")]
        public IActionResult VerClase(int idMaestro)
        {
            try
            {
                int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
                var clase = service.GetClaseByMaestro(idMaestro, idUsuario);
                return Ok(clase);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Maestro")]
        public IActionResult AgregarClase(AgregarClaseDTO dto)
        {
            try
            {
                int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
                var validation = agregarValidator.Validate(dto);
                if (!validation.IsValid)
                {
                    return BadRequest(validation.Errors.Select(e => e.ErrorMessage));
                }
                service.AgregarClase(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = "Maestro")]
        public IActionResult EditarClase(EditarClaseDTO dto)
        {
            try
            {
                int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int idUsuario);
                var validation = editarValidator.Validate(dto);
                if (!validation.IsValid)
                {
                    return BadRequest(validation.Errors.Select(e => e.ErrorMessage));
                }
                service.EditarClase(dto, idUsuario);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
