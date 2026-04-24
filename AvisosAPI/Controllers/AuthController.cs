using AvisosAPI.Models.DTOs;
using AvisosAPI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AvisosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService authService;
        private readonly IValidator<LoginDTO> validator;

        public AuthController(AuthService authService, IValidator<LoginDTO> validator)
        {
            this.authService = authService;
            this.validator = validator;
        }

        [HttpPost]
        public IActionResult Login(LoginDTO dto)
        {
            try
            {
                var validation = validator.Validate(dto);
                if (validation.IsValid)
                {
                    var res = authService.Login(dto);
                    return Ok(res);
                }
                else
                {
                    return BadRequest(validation.Errors.Select(x => x.ErrorMessage));
                }
            }
            catch (KeyNotFoundException)
            {
                return Unauthorized(new List<string> { "Usuario o contraseña incorrectos" });
            }
            catch (Exception ex)
            {
                return BadRequest(new List<string> { ex.Message });
            }
        }
    }
}
