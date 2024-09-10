using Application.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class AddRecieptDTOValidator:AbstractValidator<AddRecieptDTO>    
    {
        public AddRecieptDTOValidator()
        {
            RuleFor(p=>p.Name).NotNull().NotEmpty().WithMessage("Name is required");
            RuleFor(p=>p.Portions).NotNull().NotEmpty().GreaterThan(0).WithMessage("Portions is required");
            RuleFor(p=>p.Kkal).NotNull().NotEmpty().GreaterThan(0).WithMessage("KKal is required");
            RuleFor(p=>p.CookDuration).GreaterThan(new TimeSpan(0)).WithMessage("Duration is required");
        }
    }
}
