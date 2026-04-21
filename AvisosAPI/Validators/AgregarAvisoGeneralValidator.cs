using AvisosAPI.Models.DTOs;
using FluentValidation;

namespace AvisosAPI.Validators
{
    public class AgregarAvisoGeneralValidator:AbstractValidator<AgregarAvisoGeneralDTO>
    {
        public AgregarAvisoGeneralValidator()
        {
            RuleFor(x => x.Titulo).NotEmpty().WithMessage("Ingrese un título del aviso personal");
            RuleFor(x => x.Contenido).NotEmpty().WithMessage("Ingrese un contenido del aviso personal");
            RuleFor(x => x.IdMaestro).GreaterThan(0).WithMessage("Seleccione un maestro");
            RuleFor(x => x.FechaVigencia).Must(x => x.Date >= DateTime.Today).WithMessage("La fecha no puede ser anterior a hoy"); //sin considerar hora solo día
            /*RuleFor(x => x.FechaVigencia).GreaterThan(DateTime.Now).WithMessage("Ingrese una fecha de vigencia igual o posterior a hoy");*/ //considerando hora
        }
    }
}
