using BLL.DTO;
using FluentValidation;

namespace BLL.Validators
{
    public class ProductInfoValidator:AbstractValidator<ProductInfoModel>
    {
        public ProductInfoValidator()
        {
            RuleFor(x => x.ProductId).NotNull().NotEmpty();
            
            RuleFor(x => x.Count).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
