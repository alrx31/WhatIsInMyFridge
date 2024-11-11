using Application.Exceptions;
using Application.UseCases.Comands;
using Application.UseCases.Handlers.Comands;
using Bogus;
using Domain.Entities;
using Domain.Repository;
using FluentAssertions;
using MediatR;
using Moq;

namespace Tests.UnitTests.UseCases.Commands
{
    public class DeleteUserCommandTests
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<ICacheRepository> _cacheRepository;
        private readonly Mock<IUnitOfWork> _unitOfWork;

        private readonly DeleteUserCommandHandler _handler;

        public DeleteUserCommandTests()
        {
            _userRepository = new Mock<IUserRepository>();
            _cacheRepository = new Mock<ICacheRepository>();

            _unitOfWork = new Mock<IUnitOfWork>();

            _unitOfWork.SetupGet(x => x.UserRepository).Returns(_userRepository.Object);
            _unitOfWork.SetupGet(x => x.CacheRepository).Returns(_cacheRepository.Object);

            _handler = new DeleteUserCommandHandler(
                _unitOfWork.Object
            );
        }


        [Fact]
        public async Task Handle_Success_WhenUserExistsAndHasRights()
        {
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

            var command = new DeleteUserCommand
            {
                Id = userId,
                InitiatorId = userId
            };

            _cacheRepository.Setup(x => x.GetCacheData<User>($"user-{userId}"))
               .ReturnsAsync(existingUser);

            _userRepository.Setup(x => x.GetUserById(userId))
                .ReturnsAsync(existingUser);

            _userRepository.Setup(x => x.GetTokenModel(existingUser.email))
                .ReturnsAsync((RefreshTokenModel)null);
            _userRepository.Setup(x => x.DeleteUser(userId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            _userRepository.Verify(x => x.DeleteUser(userId), Times.Once);
            _cacheRepository.Verify(x => x.RemoveCacheData($"user-{userId}"), Times.Once);
            _unitOfWork.Verify(x => x.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_Fail_WhenInitiatorDoesNotHaveAccess()
        {
            // Arrange
            var faker = new Faker();
            var userId = faker.Random.Int();
            var initiatorId = userId + 1;

            var existingUser = new User
            {
                id = userId,
                email = faker.Internet.Email(),
                login = faker.Internet.UserName(),
                name = faker.Name.FullName(),
                password = "oldPasswordHash",
                isAdmin = false
            };

            var command = new DeleteUserCommand
            {
                Id = userId,
                InitiatorId = initiatorId
            };

            _cacheRepository.Setup(x => x.GetCacheData<User>($"user-{initiatorId}"))
                .ReturnsAsync(existingUser);

            _userRepository.Setup(x => x.GetUserById(userId))
                .ReturnsAsync(existingUser);

            _userRepository.Setup(x => x.GetTokenModel(existingUser.email))
                .ReturnsAsync((RefreshTokenModel)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("You do not have acess");

            _userRepository.Verify(x => x.DeleteUser(userId), Times.Never);
            _cacheRepository.Verify(x => x.RemoveCacheData($"user-{userId}"), Times.Never);
            _unitOfWork.Verify(x => x.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_Fail_WhenUserNotFound()
        {
            // Arrange
            var userId = 1;
            var initiatorId = 2;

            var command = new DeleteUserCommand(userId, initiatorId);

            _cacheRepository.Setup(x => x.GetCacheData<User>($"user-{initiatorId}"))
                .ReturnsAsync((User)null);

            _userRepository.Setup(x => x.GetUserById(userId))
                .ReturnsAsync((User)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("User not found");

            _userRepository.Verify(x => x.DeleteUser(It.IsAny<int>()), Times.Never);
        }

    }
}
