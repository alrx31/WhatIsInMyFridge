using BLL.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
