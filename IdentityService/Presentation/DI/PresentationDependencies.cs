using Microsoft.Extensions.DependencyInjection;
using Presentation.ExceptionsHandlingMiddleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.DI
{
    public static class PresentationDependencies
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            // HttpContextAccessor
            services.AddHttpContextAccessor();

            // Middleware
            services.AddScoped<ExceptionHandlingMiddleware>();

            return services;
        }
    }
}
