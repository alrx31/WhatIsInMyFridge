using Application.Exceptions;
using Application.UseCases.Comands;
using Application.UseCases.Handlers.Comands;
using AutoMapper;
using Bogus;
using Domain.Entities;
using Domain.Repository;
using FluentAssertions;
using Infastructure.Persistanse;
using Infastructure.Services;
using MediatR;
using Moq;

namespace Tests.IdentityServiceTests.UnitTests.UseCases.Commands
{
    public class UserRegisterComandTests
    {
        private readonly Mock<IJWTService> _jwtService;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<ICacheRepository> _cacheRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly UserRegisterComandHandler _handler;

        public UserRegisterComandTests()
        {
            _jwtService = new Mock<IJWTService>();
            _userRepository = new Mock<IUserRepository>();
            _cacheRepository = new Mock<ICacheRepository>();
            _mapper = new Mock<IMapper>();

            _unitOfWork = new Mock<IUnitOfWork>();

            _unitOfWork.SetupGet(x => x.UserRepository).Returns(_userRepository.Object);
            _unitOfWork.SetupGet(x => x.CacheRepository).Returns(_cacheRepository.Object);

            _handler = new UserRegisterComandHandler(
                _jwtService.Object,
                _unitOfWork.Object,
                _mapper.Object
            );
        }

        [Fact]
        public async Task Handle_Success_WhenDataCorrect()
        {
            // Arrange
            var faker = new Faker();
            var command = new UserRegisterCommand(
                faker.Internet.UserName(),
                faker.Internet.UserName(),
                faker.Internet.Email(),
                faker.Internet.Password()
            );

            var user = new User
            {
                name = command.Name,
                login = command.Login,
                email = command.Email,
                password = Scripts.GetHash(command.Password)
            };

            _unitOfWork.SetupSequence(x => x.UserRepository.GetUserByLogin(command.Login))
                .ReturnsAsync((User)null)
                .ReturnsAsync(user);

            _mapper.Setup(x => x.Map<User>(command)).Returns(user);

            _unitOfWork.Setup(x => x.UserRepository.RegisterUser(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            _unitOfWork.Setup(x => x.UserRepository.AddRefreshTokenField(It.IsAny<RefreshTokenModel>()))
                .Returns(Task.CompletedTask);

            _unitOfWork.Setup(x => x.CompleteAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            _unitOfWork.Verify(x => x.UserRepository.RegisterUser(It.IsAny<User>()), Times.Once);
            _unitOfWork.Verify(x => x.CompleteAsync(), Times.Exactly(2)); // Сохранение данных происходит дважды
        }

        [Fact]
        public async Task Handle_Fail_WhenUserAlreadyExists()
        {
            // Arrange
            var faker = new Faker();
            var command = new UserRegisterCommand
            (
                faker.Internet.UserName(),
                faker.Internet.UserName(),
                faker.Internet.Email(),
                faker.Internet.Password()
            );

            var existingUser = new User
            {
                id = faker.Random.Int(),
                email = command.Email,
                login = command.Login
            };

            _unitOfWork.Setup(x => x.UserRepository.GetUserByLogin(command.Login))
                .ReturnsAsync(existingUser); // Пользователь с таким логином уже существует

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<AlreadyExistsException>()
                .WithMessage("This Login is not avaible");

            _unitOfWork.Verify(x => x.UserRepository.RegisterUser(It.IsAny<User>()), Times.Never);
        }
    }
}
