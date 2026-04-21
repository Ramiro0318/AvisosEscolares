namespace AvisosAPI.Models.DTOs
{
    public class AgregarClaseDTO
    {
        public int IdMaestro { get; set;  }
        public string Nombre { get; set; } = null!;
    }
    public class EditarClaseDTO: AgregarClaseDTO
    {

    }
    public class ClaseDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public List<AlumnoListaDTO> ListaAlumnos { get; set; } = null!;
    }
}
