using Bogus;
using Domain.Entities;
using FluentAssertions;
using Infastructure.Persistanse;
using Moq;
using StackExchange.Redis;
using System.Text.Json;

namespace Tests.Repositories
{
    public class CacheRepositoryTests
    {
        private readonly Mock<IConnectionMultiplexer> _redis;
        private readonly Mock<IDatabase> _database;

        private readonly CacheRepository _repository;

        public CacheRepositoryTests()
        {
            _redis = new Mock<IConnectionMultiplexer>();
            _database = new Mock<IDatabase>();

            _redis.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object?>())).Returns(_database.Object);

            _repository = new CacheRepository(_redis.Object);
        }

        [Fact]
        public async Task GetCacheData_Success_ReturnsValue()
        {
            var faker = new Faker();
            var key = new Faker().Random.String();
            var user = new User
            {
                id = faker.Random.Int(),
                email = faker.Internet.Email(),
                login = faker.Internet.UserName(),
                name = faker.Name.FullName(),
                password = new Faker().Internet.Password()
            };

            _database.Setup(x => x.StringGetAsync(key, It.IsAny<CommandFlags>()))
                .ReturnsAsync(JsonSerializer.Serialize(user));

            var result = await _repository.GetCacheData<User>(key);

            result.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task GetCacheData_Fail_ShouldReturnsNull()
        {
            var key = new Faker().Random.String();

            _database.Setup(x => x.StringGetAsync(key, It.IsAny<CommandFlags>()))
                .ReturnsAsync(RedisValue.Null);

            var result = await _repository.GetCacheData<User>(key);

            result.Should().BeNull();
        }

        [Fact]
        public async Task RemoveCacheData_Success_ShouldCallDelete()
        {
            var key = new Faker().Random.String();

            await _repository.RemoveCacheData(key);

            _database.Verify(x => x.KeyDeleteAsync(key, It.IsAny<CommandFlags>()), Times.Once);
        }

        [Fact]
        public async Task GetCacheData_AfterSettingData_ShouldReturnValue()
        {
            var faker = new Faker();
            var key = faker.Internet.UserName();

            var user = new User
            {
                id = faker.Random.Int(),
                email = faker.Internet.Email(),
                login = faker.Internet.UserName(),
                name = faker.Name.FullName(),
                password = faker.Internet.Password(),
                isAdmin = faker.Random.Bool()
            };

            TimeSpan? expiry = TimeSpan.FromMinutes(5); 
            var serializedData = JsonSerializer.Serialize(user);
            var redisValue = new RedisValue(serializedData);

            _database.Setup(x => x.StringSetAsync(
                key,
                redisValue,
                expiry,
                It.IsAny<When>(),
                It.IsAny<CommandFlags>()))
            .ReturnsAsync(true);

            await _repository.SetCatcheData(key, user, expiry);

            _database.Setup(x => x.StringGetAsync(key, It.IsAny<CommandFlags>()))
                .ReturnsAsync(redisValue);

            var result = await _repository.GetCacheData<User>(key);
            result.Should().BeEquivalentTo(user);
        }
    }
}
