using Application.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class RegisterDTOValidator:AbstractValidator<RegisterDTO>
    {
        public RegisterDTOValidator()
        {
            RuleFor(x => x.login).NotNull().NotEmpty().WithMessage("Login is required");
            RuleFor(x => x.password).NotNull().NotEmpty().WithMessage("Password is required");
            RuleFor(x => x.name).NotNull().NotEmpty().WithMessage("Name is required");
            //TODO: Uncoment for production
            //RuleFor(x => x.password).MinimumLength(6).WithMessage("Password must be at least 6 characters");
            RuleFor(x => x.email).NotNull().NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.email).EmailAddress().WithMessage("Email is not valid");
        }
    }
}
