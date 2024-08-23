using Application.DTO;
using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class LogoutDTOValidator:AbstractValidator<LogoutDTO>
    {
        public LogoutDTOValidator() { 
            RuleFor(x=> x.UserId).NotNull().NotEmpty().WithMessage("Id cant be null");
        }
    }
}
