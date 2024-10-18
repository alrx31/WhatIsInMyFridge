using Application.Exceptions;
using Application.UseCases.Handlers.Queries;
using Application.UseCases.Queries;
using Bogus;
using Domain.Entities;
using Domain.Repository;
using FluentAssertions;
using Moq;

namespace Tests.IdentityServiceTests.UnitTests.UseCases.Queries
{
    public class GetUserByIdQueryTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new Mock<IUnitOfWork>();
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<ICacheRepository> _cacheRepository;

        private readonly GetUserByIdQueryHandler _handler;

        public GetUserByIdQueryTests()
        {
            _userRepository = new Mock<IUserRepository>();
            _cacheRepository = new Mock<ICacheRepository>();

            _unitOfWork = new Mock<IUnitOfWork>();

            _unitOfWork.SetupGet(x => x.UserRepository).Returns(_userRepository.Object);
            _unitOfWork.SetupGet(x => x.CacheRepository).Returns(_cacheRepository.Object);

            _handler = new GetUserByIdQueryHandler(
                _unitOfWork.Object
                );
        }

        [Fact]
        public async Task Handle_Success_WhenUserExist()
        {
            var faker = new Faker();

            var user = new User
            {
                id = faker.Random.Int(),
                name = faker.Name.FullName(),
                email = faker.Internet.Email(),
                password = faker.Internet.Password()
            };

            var query = new GetUserQueryByIdQuery(user.id);

            _userRepository.Setup(x => x.GetUserById(user.id)).ReturnsAsync(user);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(user);
            result.id.Should().Be(user.id);
        }

        [Fact]
        public async Task Handle_Fail_WhenUserNotFound()
        {
            var faker = new Faker();

            var query = new GetUserQueryByIdQuery(faker.Random.Int());

            _userRepository.Setup(x => x.GetUserById(query.Id)).ReturnsAsync((User)null);

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
