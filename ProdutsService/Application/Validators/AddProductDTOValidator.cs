using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using FluentValidation;

namespace Application.Validators
{
    public class AddProductDTOValidator:AbstractValidator<AddProductDTO>
    {
        public AddProductDTOValidator()
        {
            RuleFor(p=>p.Name).NotNull().NotEmpty().WithMessage("Name is required");
            RuleFor(p=>p.PricePerKilo).NotNull().GreaterThan(0).WithMessage("Price must be greater than 0");
            RuleFor(p=>p.PricePerKilo).NotNull().GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }
}
