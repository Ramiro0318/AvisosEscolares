using System;
using System.Collections.Generic;

namespace AvisosAPI.Models.Entities;

public partial class Maestro
{
    public int Id { get; set; }

    public string NumControl { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Correo { get; set; }

    public string Contraseña { get; set; } = null!;

    public DateTime? UltimaVistaBandeja { get; set; }

    public virtual ICollection<Avisogeneral> Avisogeneral { get; set; } = new List<Avisogeneral>();

    public virtual ICollection<Avisopersonal> Avisopersonal { get; set; } = new List<Avisopersonal>();

    public virtual ICollection<Clase> Clase { get; set; } = new List<Clase>();
}
