using AvisosAPI.Models.DTOs;
using AvisosAPI.Models.Entities;
using AvisosAPI.Repositories;
using FluentValidation;

namespace AvisosAPI.Validators
{
    public class EditarAlumnoValidator:AbstractValidator<EditarAlumnoDTO>
    {
        private readonly Repository<Alumno> repos;

        public EditarAlumnoValidator(Repository<Alumno> repos)
        {
            RuleFor(x => x.Correo).EmailAddress().WithMessage("Ingrese un correo válido").When(x => !string.IsNullOrWhiteSpace(x.Correo)).MaximumLength(100).WithMessage("Ingrese un correo de máximo 100 caracteres");
            RuleFor(x => x.NumControl).NotEmpty().WithMessage("Ingrese un número de control").Length(6).WithMessage("El número de control debe ser de 6 digitos.");
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("Ingrese un nombre del alumno").WithMessage("Ingrese un nombre de máximo 100 caracteres");
            RuleFor(x => x.Contraseña).NotEmpty().WithMessage("Ingrese una contraseña").MaximumLength(50).WithMessage("Ingrese una contraseña de máximo 50 caracteres");
            RuleFor(x => x).Must(NumControlRepetido).WithMessage("Ya existe un alumno con el mismo número de control");
            this.repos = repos;
        }

        private bool NumControlRepetido(EditarAlumnoDTO dto)
        {
            return !repos.GetAll().Any(x => x.NumControl == dto.NumControl && x.Id != dto.Id);
        }
    }
}
