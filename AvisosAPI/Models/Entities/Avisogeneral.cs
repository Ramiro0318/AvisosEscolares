using System;
using System.Collections.Generic;

namespace AvisosAPI.Models.Entities;

public partial class Avisogeneral
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Contenido { get; set; } = null!;

    public int IdMaestro { get; set; }

    public DateTime FechaVigencia { get; set; }

    public DateTime FechaCreacion { get; set; }

    public sbyte Recibido { get; set; }

    public sbyte Eliminado { get; set; }

    public virtual Maestro IdMaestroNavigation { get; set; } = null!;
}
