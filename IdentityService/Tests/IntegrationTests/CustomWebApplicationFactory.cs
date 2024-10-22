using Infastructure.Persistanse;
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

        public CustomWebApplicationFactory(string postgreSqlConnection, string redis)
        {
            _postgreSqlConnection = postgreSqlConnection;
            _redis = redis;
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
            });
        }
    }
}
