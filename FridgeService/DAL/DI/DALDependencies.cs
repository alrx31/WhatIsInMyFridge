using DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using DAL.Persistanse;
using Microsoft.EntityFrameworkCore;
using DAL.Persistanse.Protos;
using DAL.IRepositories;
using StackExchange.Redis;
using Microsoft.AspNetCore.Builder;


namespace DAL.DI
{
    public static  class DALDependencies
    {
        public static IServiceCollection AddDALDependencies(this IServiceCollection services)
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"));
            });

            services.AddScoped<IFridgeRepository, FridgeRepository>();
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFridgeRepository,FridgeRepository>();
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
