using DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using DAL.Persistanse;
using Microsoft.EntityFrameworkCore;
using DAL.Persistanse.Protos;
using DAL.IRepositories;
using StackExchange.Redis;
using Microsoft.AspNetCore.Builder;
using DAL.Interfaces;


namespace DAL.DI
{
    public static  class DALDependencies
    {
        public static IServiceCollection AddDALDependencies(this IServiceCollection services,IConfiguration configuration)
        {
            // redis
            services.AddScoped<IConnectionMultiplexer>(_ =>
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFridgeRepository,FridgeRepository>();
            services.AddScoped<IgRPCService, gRPCService>();
            services.AddScoped<IProductsgRPCService, ProductsgRPCService>();

            services.AddScoped<IKafkaProducer, KafkaProducer>();


            services.AddGrpcClient<Greeter.GreeterClient>(o =>
            {
                o.Address = new Uri("http://identityservice:8081");
            });

            services.AddGrpcClient<Products.ProductsClient>(o =>
            {
                o.Address = new Uri("http://host.docker.internal:8083");
            });

            return services;
        }
    }
}
