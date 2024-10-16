using DotNet.Testcontainers.Builders;
using Infastructure.Persistanse;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace Tests.IntegrationTests
{
    public abstract class ControllerTests
    {
        protected readonly HttpClient _httpClient;
        protected readonly FakeUsersGenerator _fakeUsersGenerator;

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

            postgreSqlContainerTask.Wait();
            redisContainerTask.Wait();

            var factory = new CustomWebApplicationFactory<Program>(postgreSqlContainer.GetConnectionString(),
                redisContainer.GetConnectionString());

            _fakeUsersGenerator = new FakeUsersGenerator();

            using (var scope = factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                dbContext.Database.Migrate();

                InitializeDatabase(dbContext);
            }
            

            _httpClient = factory.CreateClient();
        }

        protected void InitializeDatabase(ApplicationDbContext dataContext) {
            
            _fakeUsersGenerator.InitializeData();
            dataContext.AddRange(_fakeUsersGenerator.Users);

            dataContext.SaveChanges();
        }
    }
}
