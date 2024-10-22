using Confluent.Kafka;
using Domain.Repository;
using Infrastructure.Persistance;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI
{
    public static class InfrastructureDI
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
            services.AddSingleton<ApplicationDbContext>();


            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IListRepository, ListRepository>();
            services.AddScoped<IRecieptsRepository, RecieptsRepository>();
            services.AddScoped<IListManageRepository, ListManageRepository>();


            services.AddHostedService<KafkaConsumerService>();

            services.AddGrpc();


            return services;
        }
    }
}
