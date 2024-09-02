using BLL.MappingProfiles;
using BLL.Services;
using DAL.Persistanse;
using DAL.Repositories;
using DAL.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.DI
{
    public static class BLLdependencies
    {
        public static IServiceCollection AddBLLDependencies(this IServiceCollection services)
        {
            services.AddScoped<IFridgeService, FridgeService>();

            services.AddAutoMapper(typeof(FridgeProfile));
            return services;
        }
    }
}
