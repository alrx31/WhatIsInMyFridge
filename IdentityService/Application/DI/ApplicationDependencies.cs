using Application.MappingProfiles;
using Application.UseCases.Comands;
using Application.UseCases.Handlers.Comands;
using Application.UseCases.Handlers.Queries;
using Application.Validators;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DI
{
    public static class ApplicationDependencies
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(typeof(UserProfile));

            // MediatR
            services.AddMediatR(typeof(GetUserByIdQueryHandler).Assembly);
            services.AddMediatR(typeof(UserLogoutComandHandler).Assembly);

            services.AddTransient<IRequestHandler<UserLogoutCommand, Unit>, UserLogoutComandHandler>();


            // FluentValidation
            services.AddControllers().AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<LoginDTOValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<RegisterDTOValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<RefreshTokenDTOValidator>();
            });

            return services;
        }
    }
}
