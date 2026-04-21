using System;
using System.Collections.Generic;

namespace AvisosAPI.Models.Entities;

public partial class Avisopersonal
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Contenido { get; set; } = null!;

    public int IdMaestro { get; set; }

    public int IdAlumno { get; set; }

    public DateTime? FechaLectura { get; set; }

    public DateTime FechaCreacion { get; set; }

    public bool Recibido { get; set; }

    public bool Eliminado { get; set; }

    public virtual Alumno IdAlumnoNavigation { get; set; } = null!;

    public virtual Maestro IdMaestroNavigation { get; set; } = null!;
}
