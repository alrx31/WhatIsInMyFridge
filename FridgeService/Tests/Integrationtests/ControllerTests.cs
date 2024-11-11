using DAL.Persistanse;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Testcontainers.Kafka;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace Tests.IntegrationTests
{
    public abstract class ControllerTests
    {
        protected readonly HttpClient _httpClient;
        protected readonly FakeFridgeGenerator _fakeFridgeGenerator;

        public ControllerTests()
        {
            var network = new NetworkBuilder()
                .Build();

            var postgreSqlContainer = new PostgreSqlBuilder()
                .Build();

            var postgreSqlContainerTask = postgreSqlContainer.StartAsync();

            var redisContainer = new RedisBuilder()
                .Build();

            var redisContainerTask = redisContainer.StartAsync();

            var kafkaContainer = new KafkaBuilder()
                .WithEnvironment("KAFKA_ACKS", "0")
                .Build();
            
            var kafkaContainerTask = kafkaContainer.StartAsync();

            postgreSqlContainerTask.Wait();
            redisContainerTask.Wait();
            kafkaContainerTask.Wait();


            var factory = new CustomWebApplicationFactory<Program>(postgreSqlContainer.GetConnectionString(),
                redisContainer.GetConnectionString(),
                kafkaContainer.GetBootstrapAddress());

            _fakeFridgeGenerator = new FakeFridgeGenerator();

            using (var scope = factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                dbContext.Database.Migrate();

                InitializeDatabase(dbContext);
            }


            _httpClient = factory.CreateClient();
        }

        protected abstract void InitializeDatabase(ApplicationDbContext dataContext);
    }
}
