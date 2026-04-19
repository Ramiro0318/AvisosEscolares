using System;
using System.Collections.Generic;

namespace AvisosAPI.Models.Entities;

public partial class Clase
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdMaestro { get; set; }

    public virtual ICollection<Alumno> Alumno { get; set; } = new List<Alumno>();

    public virtual Maestro IdMaestroNavigation { get; set; } = null!;
}
