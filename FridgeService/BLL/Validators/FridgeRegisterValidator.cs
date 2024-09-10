using BLL.DTO;
using FluentValidation;

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
