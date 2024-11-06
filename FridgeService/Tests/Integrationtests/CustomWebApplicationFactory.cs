using Confluent.Kafka;
using DAL.Entities;
using DAL.Persistanse;
using DAL.Persistanse.Protos;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.IntegrationTests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        private readonly string _postgreSqlConnection;
        private readonly string _redis;
        private readonly string _bootstrapAddress;

        public CustomWebApplicationFactory(string postgreSqlConnection, string redis, string bootstrapAddres)
        {
            _postgreSqlConnection = postgreSqlConnection;
            _redis = redis;
            _bootstrapAddress = bootstrapAddres;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var serviceDesciptor = services.SingleOrDefault(serviceDesciptor =>
                    serviceDesciptor.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (serviceDesciptor is not null)
                {
                    services.Remove(serviceDesciptor);
                }

                services.AddDbContextPool<ApplicationDbContext>(dbContextOptions => dbContextOptions.UseNpgsql(_postgreSqlConnection));

                services.AddStackExchangeRedisCache(redisCacheOptions =>
                {
                    redisCacheOptions.Configuration = _redis;
                });

                services.AddSingleton<IProducer<Null, string>>(provider =>
                {
                    var config = new ProducerConfig { BootstrapServers = _bootstrapAddress };
                    return new ProducerBuilder<Null, string>(config).Build();
                });

                services.AddHangfire(configuration =>
                    configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                                 .UseSimpleAssemblyNameTypeSerializer()
                                 .UseRecommendedSerializerSettings()
                                 .UsePostgreSqlStorage(_postgreSqlConnection));

                services.AddHangfireServer();

                // Add gRPC Client
                services.AddGrpcClient<Greeter.GreeterClient>(options =>
                {
                    options.Address = new Uri("https://localhost:5001"); // gRPC endpoint
                });

                services.AddGrpcClient<Products.ProductsClient>(o =>
                {
                    o.Address = new Uri("http://localhost:5001");
                });
            });
        }
    }
}
