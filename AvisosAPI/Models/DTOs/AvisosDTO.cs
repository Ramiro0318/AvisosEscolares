namespace AvisosAPI.Models.DTOs
{
    public class AvisosNuevosAlumnosDTO
    {
        public int AvisosPersonalesNuevos { get; set; }
        public int AvisosGeneralesNuevos { get; set; }
    }

    public class AvisosNuevosMaestroDTO
    {
        public int AvisosGeneralesNuevos { get; set; }
    }

    public class AgregarAvisoPersonalDTO
    {
        public int IdMaestro { get; set; }
        public int IdAlumno { get; set;  }
        public string Titulo { get; set; } = null!;
        public string Contenido { get; set; } = null!;
    }
    public class AgregarAvisoGeneralDTO
    {
        public int IdMaestro { get; set; }
        public string Titulo { get; set; } = null!;
        public string Contenido { get; set; } = null!;
        public DateTime FechaVigencia { get; set; }
    }

    public class AvisoPersonalAlumnoDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public DateTime FechaCreacion { get; set; } 
        public bool Nuevo { get; set; }
        public bool Leido { get; set; }
    }
    public class AvisoPersonalMaestroDTO
    {
        public int Id { get; set;  }
        public string Titulo { get; set; } = null!;
        public DateTime? FechaLectura { get; set; }
        public DateTime FechaCreacion { get; set; }
        //public bool Nuevo { get; set; }
    }
    public class AvisoPersonalDetallesDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Contenido { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaLectura { get; set; }
        public string Maestro { get; set; } = null!;
    }

    public class AvisoGeneralDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Contenido { get; set; } = null!;
        public bool Nuevo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaVigencia { get; set; }
        public string Maestro { get; set; } = null!; 
    }
}
