using AvisosAPI.Models.DTOs;
using AvisosAPI.Models.Entities;
using AvisosAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AvisosAPI.Services
{
    public class AuthService
    {
        private readonly Repository<Alumno> alumnoRepository;
        private readonly Repository<Maestro> maestroRepository;
        private readonly IConfiguration configuration;

        public AuthService(Repository<Alumno> alumnoRepository, Repository<Maestro> maestroRepository, IConfiguration configuration)
        {
            this.alumnoRepository = alumnoRepository;
            this.maestroRepository = maestroRepository;
            this.configuration = configuration;
        }
        public LoginRespuestaDTO Login(LoginDTO dto)
        {
            var maestro = maestroRepository.Query().Include(x => x.Clase).FirstOrDefault(x => x.NumControl.ToLower() == (dto.NumControl??"").ToLower() && x.Contraseña == dto.Contraseña);
            if (maestro != null)
            {
                var claims = new List<Claim>()
                {
                    new Claim("Id", maestro.Id.ToString()),
                    new Claim("Nombre", maestro.Nombre),
                    new Claim("IdClase", maestro.Clase.FirstOrDefault()?.Id.ToString() ?? "0"),
                    new Claim("NumControl", maestro.NumControl),
                    new Claim("Correo", maestro.Correo ?? ""),
                };
                var token = GenerarJWT(claims);
                LoginRespuestaDTO res = new LoginRespuestaDTO
                {
                    Token = token,
                    Correo = maestro.Correo,
                    Id = maestro.Id,
                    IdClase = maestro.Clase.FirstOrDefault()?.Id ?? 0,
                    Nombre = maestro.Nombre,
                    NumControl = maestro.NumControl,
                    Rol = "Maestro"
                };
                return res;
            }
            var alumno = alumnoRepository.GetAll().FirstOrDefault(x => x.NumControl.ToLower() == (dto.NumControl ?? "").ToLower() && x.Contraseña == dto.Contraseña);
            if (alumno != null)
            {
                var claims = new List<Claim>()
                {
                    new Claim("Id", alumno.Id.ToString()),
                    new Claim("Nombre", alumno.Nombre),
                    new Claim("IdClase", alumno.IdClase.ToString() ?? "0"),
                    new Claim("NumControl", alumno.NumControl),
                    new Claim("Correo", alumno.Correo ?? ""),
                };
                var token = GenerarJWT(claims);
                LoginRespuestaDTO res = new LoginRespuestaDTO
                {
                    Token = token,
                    Correo = alumno.Correo,
                    Id = alumno.Id,
                    Nombre = alumno.Nombre,
                    NumControl = alumno.NumControl,
                    Rol = "Alumno"
                };
                return res;
            }
            throw new KeyNotFoundException();
        }

        private string GenerarJWT(List<Claim> claims)
        {
            var key = configuration.GetValue<string>("Jwt:SecretKey");

            var tokenDescriptor = new JwtSecurityToken(issuer: configuration.GetValue<string>("Jwt:Issuer"),
                audience: configuration.GetValue<string>("Jwt:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key ?? "")),
                SecurityAlgorithms.HmacSha256)
                );

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(tokenDescriptor);
        }
    }
}
