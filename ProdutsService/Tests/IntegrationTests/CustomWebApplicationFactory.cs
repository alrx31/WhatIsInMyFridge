using Infrastructure.Persistance;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using static StackExchange.Redis.Role;

namespace Tests.IntegrationTests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        private readonly string _mongoConnection;
        private readonly string _redis;
        private readonly string _bootstrapAddress;

        public CustomWebApplicationFactory(string mongoConnection, string redis, string bootstrapAddress)
        {
            _mongoConnection = mongoConnection;
            _redis = redis;
            _bootstrapAddress = bootstrapAddress;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var serviceDesciptor = services.SingleOrDefault(serviceDesciptor =>
                    serviceDesciptor.ServiceType == typeof(IMongoDatabase));


                if (serviceDesciptor is not null)
                {
                    services.Remove(serviceDesciptor);
                }

                services.AddSingleton(new MongoClient(_mongoConnection).GetDatabase("products-service"));

                services.Configure<MongoDbSettings>(options =>
                {
                    options.ConnectionString = _mongoConnection;
                    options.DatabaseName = "products-service";
                });

                services.AddSingleton<ApplicationDbContext>();

                services.AddStackExchangeRedisCache(redisCacheOptions =>
                {
                    redisCacheOptions.Configuration = _redis;
                });


            });
        }
    }
}