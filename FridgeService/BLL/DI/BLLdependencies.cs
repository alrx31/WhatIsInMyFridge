using BLL.DTO;
using BLL.MappingProfiles;
using BLL.Services;
using BLL.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BLL.DI
{
    public static class BLLdependencies
    {
        public static IServiceCollection AddBLLDependencies(this IServiceCollection services)
        {
            services.AddScoped<IFridgeService, FridgeService>();

            services.AddTransient<IValidator<FridgeAddDTO>, FridgeRegisterValidator>();
            services.AddTransient<IValidator<ProductInfoModel>, ProductInfoValidator>();
            services.AddTransient<IValidator<ProductsInfoList>, ProductsInfoListValidator>();

            services.AddAutoMapper(typeof(FridgeProfile));
            return services;
        }
    }
}
