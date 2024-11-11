using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MongoDB.Driver;
using System;
using Testcontainers.Kafka;
using Testcontainers.MongoDb;
using Testcontainers.Redis;

namespace Tests.IntegrationTests
{
    public abstract class ControllerTests
    {
        protected readonly HttpClient _httpClient;

        public ControllerTests()
        {
            var mongoDbContainer = new MongoDbBuilder().Build();
            var mongoDbContainerTask = mongoDbContainer.StartAsync();
            mongoDbContainerTask.Wait();

            WaitUntilMongoDbIsReady(mongoDbContainer.GetConnectionString());

            var redisContainer = new RedisBuilder().Build();
            var redisContainerTask = redisContainer.StartAsync();
            redisContainerTask.Wait();

            var kafkaContainer = new KafkaBuilder().Build();
            var kafkaContainerTask = kafkaContainer.StartAsync();
            kafkaContainerTask.Wait();

            var factory = new CustomWebApplicationFactory<Program>(
                mongoDbContainer.GetConnectionString(),
                redisContainer.GetConnectionString(),
                kafkaContainer.GetBootstrapAddress());

            var scope = factory.Services.CreateScope();
            var mongoDatabase = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

            InitializeDatabase(mongoDatabase);

            _httpClient = factory.CreateClient();
        }

        private void WaitUntilMongoDbIsReady(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var timeout = TimeSpan.FromSeconds(30);
            var start = DateTime.UtcNow;

            while (DateTime.UtcNow - start < timeout)
            {
                try
                {
                    // Attempt to list databases to confirm MongoDB is ready
                    client.ListDatabases();
                    return;
                }
                catch (Exception)
                {
                    // Retry after a short delay if MongoDB isn't ready
                    Thread.Sleep(1000);
                }
            }

            throw new Exception("MongoDB did not become ready in the expected time.");
        }

    

    private void InitializeDatabase(IMongoDatabase mongoDatabase)
        {
            var usersCollection = mongoDatabase.GetCollection<Product>("products");

            var dialogsCollection = mongoDatabase.GetCollection<Reciept>("reciepts");

            var chatsCollection = mongoDatabase.GetCollection<ProductsList>("lists");
        }
    }
}
