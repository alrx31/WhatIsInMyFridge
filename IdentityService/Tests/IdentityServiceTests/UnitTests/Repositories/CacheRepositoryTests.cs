using Bogus;
using Domain.Entities;
using FluentAssertions;
using Infastructure.Persistanse;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System.Text;
using System.Text.Json;

namespace Tests.IdentityServiceTests.UnitTests.Repositories
{
    public class CacheRepositoryTests
    {
        private readonly Mock<IDistributedCache> _redis;
        private readonly CacheRepository _repository;

        public CacheRepositoryTests()
        {
            _redis = new Mock<IDistributedCache>();
            _repository = new CacheRepository(_redis.Object);
        }

        [Fact]
        public async Task GetCacheData_Success_ReturnsValue()
        {
            var faker = new Faker();
            var key = faker.Random.String();
            var user = new User
            {
                id = faker.Random.Int(),
                email = faker.Internet.Email(),
                login = faker.Internet.UserName(),
                name = faker.Name.FullName(),
                password = faker.Internet.Password()
            };

            var serializedUser = JsonSerializer.Serialize(user);
            var bytesData = Encoding.UTF8.GetBytes(serializedUser);

            _redis.Setup(x => x.GetAsync(key, It.IsAny<CancellationToken>()))
                .ReturnsAsync(bytesData);

            var result = await _repository.GetCacheData<User>(key);

            result.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task GetCacheData_Fail_ShouldReturnsNull()
        {
            var key = new Faker().Random.String();

            _redis.Setup(x => x.GetAsync(key, It.IsAny<CancellationToken>()))
                .ReturnsAsync((byte[])null);

            var result = await _repository.GetCacheData<User>(key);

            result.Should().BeNull();
        }

        [Fact]
        public async Task RemoveCacheData_Success_ShouldCallDelete()
        {
            var key = new Faker().Random.String();

            await _repository.RemoveCacheData(key);

            _redis.Verify(x => x.RemoveAsync(key, It.IsAny<CancellationToken>()), Times.Once);
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
            var bytesData = Encoding.UTF8.GetBytes(serializedData);

            _redis.Setup(x => x.SetAsync(
                key,
                bytesData,
                It.IsAny<DistributedCacheEntryOptions>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

            await _repository.SetCatcheData(key, user, expiry);

            _redis.Setup(x => x.GetAsync(key, It.IsAny<CancellationToken>()))
                .ReturnsAsync(bytesData);

            var result = await _repository.GetCacheData<User>(key);

            result.Should().BeEquivalentTo(user);
        }
    }
}
