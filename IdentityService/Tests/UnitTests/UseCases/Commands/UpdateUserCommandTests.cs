using Application.Exceptions;
using Application.UseCases.Comands;
using Application.UseCases.Handlers.Comands;
using AutoMapper;
using Bogus;
using Domain.Entities;
using Domain.Repository;
using FluentAssertions;
using Infastructure.Services;
using Moq;
using Xunit.Sdk;

namespace Tests.UnitTests.UseCases.Commands
{
    public class UpdateUserCommandTests
    {
        private readonly Mock<IJWTService> _jwtService;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<ICacheRepository> _cacheRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IUnitOfWork> _unitOfWork;

        private readonly UpdateUserCommandHandler _handler;


        public UpdateUserCommandTests()
        {
            _jwtService = new Mock<IJWTService>();
            _userRepository = new Mock<IUserRepository>();
            _cacheRepository = new Mock<ICacheRepository>();
            _mapper = new Mock<IMapper>();

            _unitOfWork = new Mock<IUnitOfWork>();

            _unitOfWork.SetupGet(x => x.UserRepository).Returns(_userRepository.Object);
            _unitOfWork.SetupGet(x => x.CacheRepository).Returns(_cacheRepository.Object);

            _handler = new UpdateUserCommandHandler(
                _jwtService.Object,
                _unitOfWork.Object,
                _mapper.Object
            );
        }

        [Fact]
        public async Task Handle_Success_WhenUserIsRegisteredAndDataValid()
        {
            // Arrange
            var faker = new Faker();
            var userId = faker.Random.Int();
            var existingUser = new User
            {
                id = userId,
                email = faker.Internet.Email(),
                login = faker.Internet.UserName(),
                name = faker.Name.FullName(),
                password = "oldPasswordHash"
            };

            var updatedUserData = new User
            {
                email = faker.Internet.Email(),
                login = faker.Internet.UserName(),
                name = faker.Name.FullName(),
                password = "newPlainPassword"
            };

            var command = new UpdateUserCommand(
                    new UserRegisterCommand(
                            faker.Internet.UserName(),
                            faker.Internet.UserName(),
                            faker.Internet.Email(),
                            faker.Internet.Password()
                    ),
                    userId
            );

            // Настройка моков
            _userRepository.Setup(x => x.GetUserById(userId))
                .ReturnsAsync(existingUser); // Существующий пользователь

            _userRepository.Setup(x => x.UpdateUser(It.IsAny<User>()))
                .ReturnsAsync(updatedUserData); // Возвращает обновленные данные

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(updatedUserData);

            _unitOfWork.Verify(x => x.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_Fail_WhenUserNotFound()
        {
            // Arrange

            var faker = new Faker();
            var userId = faker.Random.Int();
            var command = new UpdateUserCommand(
                    new UserRegisterCommand(
                            faker.Internet.UserName(),
                            faker.Internet.UserName(),
                            faker.Internet.Email(),
                            faker.Internet.Password()
                    ),
                    userId
            );

            _userRepository.Setup(x => x.GetUserById(userId))
                .ReturnsAsync((User)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert

            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
