using Application.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class LoginDTOValidator:AbstractValidator<LoginDTO>
    {
        public LoginDTOValidator()
        {
            RuleFor(x => x.login).NotNull().NotEmpty().WithMessage("Login is required");
            RuleFor(x => x.password).NotNull().NotEmpty().WithMessage("Password is required");
        }
    }
}
