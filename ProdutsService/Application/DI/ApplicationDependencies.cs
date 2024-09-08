using Application.DTO;
using Application.MappingProfiles;
using Application.UseCases.ComandsHandlers;
using Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DI
{
    public static class ApplicationDependencies
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ProductProfile));
            services.AddAutoMapper(typeof(ListProfile));
            services.AddAutoMapper(typeof(ListManageProfile));

            services.AddScoped<AddProductComandHandler>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddProductComandHandler).Assembly));

            services.AddTransient<IValidator<AddProductDTO>, AddProductDTOValidator>();


            return services;
        }
    }
}
