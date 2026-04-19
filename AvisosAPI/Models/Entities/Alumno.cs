using System;
using System.Collections.Generic;

namespace AvisosAPI.Models.Entities;

public partial class Alumno
{
    public int Id { get; set; }

    public string NumControl { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public DateTime? UltimaVistaBandeja { get; set; }

    public int? IdClase { get; set; }

    public sbyte Eliminado { get; set; }

    public virtual ICollection<Avisopersonal> Avisopersonal { get; set; } = new List<Avisopersonal>();

    public virtual Clase? IdClaseNavigation { get; set; }
}
