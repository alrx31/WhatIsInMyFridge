using BLL.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validators
{
    public class FridgeRegisterValidator:AbstractValidator<FridgeAddDTO>
    {
        public FridgeRegisterValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(100).WithMessage("asd");
            RuleFor(x => x.Model).NotNull().NotEmpty().MaximumLength(200);
            RuleFor(x=> x.BoxNumber).NotNull().GreaterThan(-1);
            RuleFor(x=> x.BoughtDate).NotNull();
        }
    }
}
