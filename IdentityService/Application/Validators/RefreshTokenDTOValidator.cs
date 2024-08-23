using Application.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class RefreshTokenDTOValidator:AbstractValidator<RefreshTokenDTO>
    {
        public RefreshTokenDTOValidator() { 
            RuleFor(x=>x.JwtToken).NotNull().NotEmpty().WithMessage("Invalid jwt token");
        }
    }
}
