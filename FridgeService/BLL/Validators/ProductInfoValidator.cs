using BLL.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Validators
{
    public class ProductInfoValidator:AbstractValidator<ProductInfoModel>
    {
        public ProductInfoValidator()
        {
            RuleFor(x => x.ProductId).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(x => x.Count).NotNull().NotEmpty().GreaterThan(0);
        }
    }
}
