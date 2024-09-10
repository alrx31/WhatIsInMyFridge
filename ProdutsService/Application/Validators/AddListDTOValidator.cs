using Application.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class AddListDTOValidator:AbstractValidator<AddListDTO>
    {
        public AddListDTOValidator()
        {
            RuleFor(p=>p.Name).NotNull().NotEmpty().WithMessage("Name is required");
            RuleFor(p=>p.Price).NotNull().NotEmpty().GreaterThan(0).WithMessage("Price is required");
            RuleFor(p=>p.Weight).NotNull().NotEmpty().GreaterThan(0).WithMessage("Weight is required");
        }
    }
}
