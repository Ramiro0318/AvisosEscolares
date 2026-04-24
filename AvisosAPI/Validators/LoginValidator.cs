using AvisosAPI.Models.DTOs;
using FluentValidation;

namespace AvisosAPI.Validators
{
    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            RuleFor(x => x.NumControl).NotEmpty().WithMessage("Escriba su numero de control");
            RuleFor(x => x.Contraseña).NotEmpty().WithMessage("Escriba la contraseña");
        }
    }
}
