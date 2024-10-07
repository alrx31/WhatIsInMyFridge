using Application.DTO;
using Application.Exceptions;
using Application.UseCases.Comands;
using Application.UseCases.Handlers.Comands;
using AutoMapper;
using Bogus;
using Domain.Entities;
using Domain.Repository;
using FluentAssertions;
using Identity.Infrastructure;
using Infastructure.Persistanse;
using Infastructure.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.UseCases
{
    public class UserLoginCommandTests
    {
        private readonly Mock<IJWTService> _jwtService;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<ICacheRepository> _cacheRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly UserLoginComandHandler _handler;

        public UserLoginCommandTests()
        {
            _jwtService = new Mock<IJWTService>();
            _userRepository = new Mock<IUserRepository>();
            _cacheRepository = new Mock<ICacheRepository>();
            _mapper = new Mock<IMapper>();

            _unitOfWork = new Mock<IUnitOfWork>();

            _unitOfWork.SetupGet(x => x.UserRepository).Returns(_userRepository.Object);
            _unitOfWork.SetupGet(x => x.CacheRepository).Returns(_cacheRepository.Object);

            _handler = new UserLoginComandHandler(
                _jwtService.Object,
                _unitOfWork.Object,
                _mapper.Object
            );
        }

        [Fact]
        public async Task Handle_Success_WhenUserIsRegistered()
        {
            // Arrange
            var faker = new Faker();

            var command = new UserLoginCommand
            (
                faker.Internet.UserName(),
                faker.Internet.Password()
            );

            var user = new User
            {
                id = faker.Random.Int(),
                email = command.Login,
                password = Scripts.GetHash(command.Password) // Здесь используется хеш пароля
            };

            var refreshTokenModel = new RefreshTokenModel
            {
                email = user.email,
                refreshToken = faker.Random.String(32),
                refreshTokenExpiryTime = DateTime.UtcNow.AddHours(12),
                userId = user.id
            };

            _unitOfWork.Setup(x => x.UserRepository.GetUserByLogin(command.Login))
                .ReturnsAsync(user); // Пользователь существует

            _unitOfWork.Setup(x => x.UserRepository.GetTokenModel(user.email))
                .ReturnsAsync(refreshTokenModel); // Возвращаем существующий refresh токен

            _unitOfWork.Setup(x => x.CompleteAsync())
                .Returns(Task.CompletedTask);

            _jwtService.Setup(x => x.GenerateJwtToken(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(faker.Random.String(128)); // Генерируем фиктивный JWT токен

            _jwtService.Setup(x => x.GenerateRefreshToken())
                .Returns(faker.Random.String(32)); // Генерируем новый refresh токен

            _mapper.Setup(x => x.Map<UserDTO>(user))
                .Returns(new UserDTO { id = user.id, email = user.email });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull(); 
            result.IsLoggedIn.Should().BeTrue();
            result.JwtToken.Should().NotBeNullOrEmpty();
            result.RefreshToken.Should().NotBeNullOrEmpty();
            result.User.Should().NotBeNull();
            result.User.email.Should().Be(user.email);

            _unitOfWork.Verify(x => x.CompleteAsync(), Times.AtLeastOnce);
            _unitOfWork.Verify(x => x.UserRepository.UpdateRefreshTokenAsync(refreshTokenModel), Times.Once);
        }

        [Fact]
        public async Task Handle_Fail_WhenUserIsNotRegistered()
        {
            var faker = new Faker();

            var command = new UserLoginCommand
            (
                faker.Internet.UserName(),
                faker.Internet.Password()
            );

            _unitOfWork.Setup(x => x.UserRepository.GetUserByLogin(command.Login))
                .ReturnsAsync((User)null); 

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<BadRequestException>();
        }
    }
}
