using BLL.DTO;
using FluentValidation;

namespace BLL.Validators
{
    public class ProductsInfoListValidator : AbstractValidator<ProductsInfoList>
    {
        public ProductsInfoListValidator()
        {
            RuleFor(x => x.ProductsInfos).NotNull();
            RuleForEach(x => x.ProductsInfos).SetValidator(new ProductInfoValidator());
        }
    }
    
}
