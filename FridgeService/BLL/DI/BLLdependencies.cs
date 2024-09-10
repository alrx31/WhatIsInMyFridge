using BLL.DTO;
using BLL.MappingProfiles;
using BLL.Services;
using BLL.Validators;
using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.DI
{
    public static class BLLdependencies
    {
        public static IServiceCollection AddBLLDependencies(this IServiceCollection services,string HangfireConnectionString)
        {
            services.AddScoped<IFridgeService, FridgeService>();

            services.AddTransient<IValidator<FridgeAddDTO>, FridgeRegisterValidator>();
            services.AddTransient<IValidator<ProductInfoModel>, ProductInfoValidator>();
            services.AddTransient<IValidator<ProductsInfoList>, ProductsInfoListValidator>();

            services.AddAutoMapper(typeof(FridgeProfile));

            services.AddHangfire(config =>
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                  .UseSimpleAssemblyNameTypeSerializer()
                  .UseDefaultTypeSerializer()
                  .UsePostgreSqlStorage(HangfireConnectionString));

            services.AddHangfireServer();

            return services;
        }
    }
}
