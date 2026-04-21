using AvisosAPI.Models.DTOs;
using FluentValidation;

namespace AvisosAPI.Validators
{
    public class EditarClaseValidator:AbstractValidator<EditarClaseDTO>
    {
        public EditarClaseValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("Ingrese un nombre de clase").MaximumLength(100).WithMessage("Escriba un nombre de clase de máximo 100 caracteres");
            RuleFor(x => x.IdMaestro).GreaterThan(0).WithMessage("Seleccione un maestro");
        }
    }
}
