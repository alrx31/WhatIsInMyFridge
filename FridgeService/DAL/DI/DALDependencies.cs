using DAL.Repositories;
using DAL.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using DAL.Persistanse;
using Microsoft.EntityFrameworkCore;


namespace DAL.DI
{
    public static  class DALDependencies
    {
        public static IServiceCollection AddDALDependencies(this IServiceCollection services)
        {
            services.AddScoped<IFridgeRepository, FridgeRepository>();
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFridgeRepository,FridgeRepository>();

            return services;
        }
    }
}
