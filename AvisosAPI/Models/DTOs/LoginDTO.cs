namespace AvisosAPI.Models.DTOs
{
    public class LoginDTO
    {
        public string? NumControl { get; set; }
        public string? Contraseña { get; set; }
    }

    public class LoginRespuestaDTO
    {
        public int Id { get; set; } 
        public string Token { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string? Correo { get; set; } 
        public string NumControl { get; set; } = null!;
        public string Rol { get; set; } = null!;
        public int IdClase { get; set;  }

    }
}
