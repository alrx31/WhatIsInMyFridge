using Application.DTO;
using Application.UseCases.Comands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class LoginDTOValidator:AbstractValidator<UserLoginCommand>
    {
        public LoginDTOValidator()
        {
            RuleFor(x => x.Login).NotNull().NotEmpty().WithMessage("Login is required");
            RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password is required");
        }
    }
}
