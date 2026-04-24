namespace AvisosAPI.Models.DTOs
{
    public class AlumnoListaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string NumControl { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string? Contraseña { get; set; }
    }
    public class AlumnoDetallesDTO:AlumnoListaDTO
    {
        //public string Correo { get; set; } = null!;
        public List<AvisoPersonalMaestroDTO> ListaAvisosAlumno { get; set; } = null!;
    }
    public class AgregarAlumnoDTO
    {
        public int IdClase { get; set; } 
        public string? NumControl { get; set; } 
        public string? Nombre { get; set; } 
        public string? Correo { get; set; }
        public string? Contraseña { get; set; }
    }
    public class EditarAlumnoDTO:AgregarAlumnoDTO
    {
        public int Id { get; set; }

    }
}
