﻿using Application.DTO;
using FluentValidation;

namespace Application.Validators
{
    public class AddProductToListDTOValidator:AbstractValidator<AddProductToListDTO>    
    {
        public AddProductToListDTOValidator()
        {
            RuleFor(p=>p.ProductId).NotNull().NotEmpty().WithMessage("Product Id is required");
            
            RuleFor(p=>p.Cost).NotNull().NotEmpty().GreaterThan(0).WithMessage("Price is required");
            
            RuleFor(p=>p.Weight).NotNull().NotEmpty().GreaterThan(0).WithMessage("Weight is required");
        }
    }
}
