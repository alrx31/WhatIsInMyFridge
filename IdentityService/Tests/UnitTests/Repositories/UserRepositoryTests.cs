using Bogus;
using Domain.Entities;
using Domain.Repository;
using FluentAssertions;
using Infastructure.Persistanse;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repositories
{
    public class UserRepositoryTests
    {
        private ApplicationDbContext _context;

        private UserRepository _repository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new UserRepository(_context);
        }

        [Fact]
        public async Task AddRefreshTokenField_Success_ShouldAddRefreshTokenField()
        {
            var faker = new Faker();

            var model = new RefreshTokenModel
            {
                userId = faker.Random.Int(),
                email = faker.Internet.Email(),
                refreshToken = faker.Random.String(),
                refreshTokenExpiryTime = faker.Date.Future()
            };

            await _repository.AddRefreshTokenField(model);

            await _context.SaveChangesAsync();

            var result = await _context.refreshTokens.FirstOrDefaultAsync(x => x.userId == model.userId);

            result.Should().NotBeNull();
            result.userId.Should().Be(model.userId);
            result.email.Should().Be(model.email);
            result.refreshToken.Should().Be(model.refreshToken);
            result.refreshTokenExpiryTime.Should().Be(model.refreshTokenExpiryTime);
        }

        [Fact]
        public async Task CanselRefreshToken_Success_ShouldCanselRefreshToken()
        {
            var faker = new Faker();

            var model = new RefreshTokenModel
            {
                userId = faker.Random.Int(),
                email = faker.Internet.Email(),
                refreshToken = faker.Random.String(),
                refreshTokenExpiryTime = faker.Date.Future()
            };

            await _repository.AddRefreshTokenField(model);

            await _context.SaveChangesAsync();

            await _repository.CanselRefreshToken(model.userId);

            await _context.SaveChangesAsync();

            var result = await _context.refreshTokens.FirstOrDefaultAsync(x => x.userId == model.userId);

            result.refreshTokenExpiryTime.Should().BeBefore(DateTime.UtcNow);
        }

        [Fact]
        public async Task DeleteUser_Success_ShouldDeleteUser()
        {
            var faker = new Faker();

            var user = new User
            {
                id = faker.Random.Int(),
                email = faker.Internet.Email(),
                login = faker.Internet.UserName(),
                name = faker.Name.FullName(),
                password = faker.Random.String()
            };

            await _context.users.AddAsync(user);

            await _context.SaveChangesAsync();

            await _repository.DeleteUser(user.id);

            await _context.SaveChangesAsync();

            var result = await _context.users.FirstOrDefaultAsync(x => x.id == user.id);

            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteRefreshTokenByUserId_Success_ShouldDeleteRefreshTokenByUserId()
        {
            var faker = new Faker();

            var model = new RefreshTokenModel
            {
                userId = faker.Random.Int(),
                email = faker.Internet.Email(),
                refreshToken = faker.Random.String(),
                refreshTokenExpiryTime = faker.Date.Future()
            };

            await _context.refreshTokens.AddAsync(model);

            await _context.SaveChangesAsync();

            await _repository.DeleteRefreshTokenByUserId(model.userId);

            await _context.SaveChangesAsync();

            var result = await _context.refreshTokens.FirstOrDefaultAsync(x => x.userId == model.userId);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetTokenModel_Success_ShouldReturnTokenModel()
        {
            var faker = new Faker();

            var model = new RefreshTokenModel
            {
                userId = faker.Random.Int(),
                email = faker.Internet.Email(),
                refreshToken = faker.Random.String(),
                refreshTokenExpiryTime = faker.Date.Future()
            };

            await _context.refreshTokens.AddAsync(model);

            await _context.SaveChangesAsync();

            var result = await _repository.GetTokenModel(model.email);

            result.Should().NotBeNull();
            result.userId.Should().Be(model.userId);
            result.email.Should().Be(model.email);
            result.refreshToken.Should().Be(model.refreshToken);
            result.refreshTokenExpiryTime.Should().Be(model.refreshTokenExpiryTime);
        }

        [Fact]
        public async Task GetUserById_Success_ShouldReturnUserById()
        {
            var faker = new Faker();

            var user = new User
            {
                id = faker.Random.Int(),
                email = faker.Internet.Email(),
                login = faker.Internet.UserName(),
                name = faker.Name.FullName(),
                password = faker.Random.String()
            };

            await _context.users.AddAsync(user);

            await _context.SaveChangesAsync();

            var result = await _repository.GetUserById(user.id);

            result.Should().NotBeNull();
            result.id.Should().Be(user.id);
            result.email.Should().Be(user.email);
            result.login.Should().Be(user.login);
            result.name.Should().Be(user.name);
            result.password.Should().Be(user.password);
        }

        [Fact]
        public async Task GetUserByLogin_Success_ShouldReturnUserByLogin()
        {
            var faker = new Faker();

            var user = new User
            {
                id = faker.Random.Int(),
                email = faker.Internet.Email(),
                login = faker.Internet.UserName(),
                name = faker.Name.FullName(),
                password = faker.Random.String()
            };

            await _context.users.AddAsync(user);

            await _context.SaveChangesAsync();

            var result = await _repository.GetUserByLogin(user.login);

            result.Should().NotBeNull();
            result.id.Should().Be(user.id);
            result.email.Should().Be(user.email);
            result.login.Should().Be(user.login);
            result.name.Should().Be(user.name);
            result.password.Should().Be(user.password);
        }

        [Fact]
        public async Task RegisterUser_Success_ShouldRegisterUser()
        {
            var faker = new Faker();

            var user = new User
            {
                id = faker.Random.Int(),
                email = faker.Internet.Email(),
                login = faker.Internet.UserName(),
                name = faker.Name.FullName(),
                password = faker.Random.String()
            };

            await _repository.RegisterUser(user);

            await _context.SaveChangesAsync();

            var result = await _context.users.FirstOrDefaultAsync(x => x.id == user.id);

            result.Should().NotBeNull();
            result.id.Should().Be(user.id);
            result.email.Should().Be(user.email);
            result.login.Should().Be(user.login);
            result.name.Should().Be(user.name);
            result.password.Should().Be(user.password);
        }

        [Fact]
        public async Task UpdateRefreshTokenAsync_Success_ShouldUpdateRefreshTokenAsync()
        {
            var faker = new Faker();

            var model = new RefreshTokenModel
            {
                userId = faker.Random.Int(),
                email = faker.Internet.Email(),
                refreshToken = faker.Random.String(),
                refreshTokenExpiryTime = faker.Date.Future()
            };

            await _context.refreshTokens.AddAsync(model);

            await _context.SaveChangesAsync();

            model.refreshToken = faker.Random.String();
            model.refreshTokenExpiryTime = faker.Date.Future();

            await _repository.UpdateRefreshTokenAsync(model);

            await _context.SaveChangesAsync();

            var result = await _context.refreshTokens.FirstOrDefaultAsync(x => x.email == model.email);

            result.refreshToken.Should().Be(model.refreshToken);
            result.refreshTokenExpiryTime.Should().Be(model.refreshTokenExpiryTime);
        }

        [Fact]
        public async Task UpdateUser_Success_ShouldUpdateUser()
        {
            var faker = new Faker();

            var user = new User
            {
                id = faker.Random.Int(),
                email = faker.Internet.Email(),
                login = faker.Internet.UserName(),
                name = faker.Name.FullName(),
                password = faker.Random.String()
            };

            await _context.users.AddAsync(user);

            await _context.SaveChangesAsync();

            user.email = faker.Internet.Email();
            user.login = faker.Internet.UserName();
            user.name = faker.Name.FullName();
            user.password = faker.Random.String();

            await _repository.UpdateUser(user);

            await _context.SaveChangesAsync();

            var result = await _context.users.FirstOrDefaultAsync(x => x.id == user.id);

            result.email.Should().Be(user.email);
            result.login.Should().Be(user.login);
            result.name.Should().Be(user.name);
            result.password.Should().Be(user.password);
        }

        [Fact]
        public async Task GetUsers_Success_ShouldReturnUsers()
        {
            var faker = new Faker();

            var users = new List<User>
            {
                new User
                {
                    id = faker.Random.Int(),
                    email = faker.Internet.Email(),
                    login = faker.Internet.UserName(),
                    name = faker.Name.FullName(),
                    password = faker.Random.String()
                },
                new User
                {
                    id = faker.Random.Int(),
                    email = faker.Internet.Email(),
                    login = faker.Internet.UserName(),
                    name = faker.Name.FullName(),
                    password = faker.Random.String()
                }
            };

            await _context.users.AddRangeAsync(users);

            await _context.SaveChangesAsync();

            var ids = users.Select(x => x.id).ToList();

            var result = await _repository.GetUsers(ids);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].id.Should().Be(users[0].id);
            result[0].email.Should().Be(users[0].email);
            result[0].login.Should().Be(users[0].login);
            result[0].name.Should().Be(users[0].name);
            result[0].password.Should().Be(users[0].password);
            result[1].id.Should().Be(users[1].id);
            result[1].email.Should().Be(users[1].email);
            result[1].login.Should().Be(users[1].login);
            result[1].name.Should().Be(users[1].name);
            result[1].password.Should().Be(users[1].password);
        }
    }
}
