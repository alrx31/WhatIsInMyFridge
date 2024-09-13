using DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using DAL.Persistanse;
using DAL.Persistanse.Protos;
using DAL.IRepositories;

namespace DAL.DI
{
    public static  class DALDependencies
    {
        public static IServiceCollection AddDALDependencies(this IServiceCollection services)
        {
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IgRPCService, gRPCService>();
            services.AddScoped<IProductsgRPCService, ProductsgRPCService>();


            services.AddGrpcClient<Greeter.GreeterClient>(o =>
            {
                o.Address = new Uri("http://localhost:8081");
            });

            services.AddGrpcClient<Products.ProductsClient>(o =>
            {
                o.Address = new Uri("http://localhost:8083");
            });

            return services;
        }
    }
}
