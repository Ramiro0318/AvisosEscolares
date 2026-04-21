using AvisosAPI.Models.DTOs;
using FluentValidation;

namespace AvisosAPI.Validators
{
    public class AgregarAvisoPersonalValidator:AbstractValidator<AgregarAvisoPersonalDTO>
    {
        public AgregarAvisoPersonalValidator()
        {
            RuleFor(x => x.Titulo).NotEmpty().WithMessage("Ingrese un título del aviso personal");
            RuleFor(x => x.Contenido).NotEmpty().WithMessage("Ingrese un contenido del aviso personal");
            RuleFor(x => x.IdMaestro).GreaterThan(0).WithMessage("Seleccione un maestro");
            RuleFor(x => x.IdAlumno).GreaterThan(0).WithMessage("Seleccione un alumno");
        }
    }
}
