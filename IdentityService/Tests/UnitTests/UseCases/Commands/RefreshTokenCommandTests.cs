using Application.Exceptions;
using Application.UseCases.Comands;
using Application.UseCases.Handlers.Comands;
using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using FluentAssertions;
using Infastructure.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace Tests.UnitTests.UseCases.Commands
{
    public class RefreshTokenCommandTests
    {
        private readonly Mock<IJWTService> _jwtService;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<ICacheRepository> _cacheRepository;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly Mock<IMapper> _mapper;

        private readonly RefreshTokenCommandHandler _handler;

        public RefreshTokenCommandTests()
        {
            _jwtService = new Mock<IJWTService>();
            _userRepository = new Mock<IUserRepository>();
            _cacheRepository = new Mock<ICacheRepository>();
            _mapper = new Mock<IMapper>();

            _unitOfWork = new Mock<IUnitOfWork>();

            _unitOfWork.SetupGet(x => x.UserRepository).Returns(_userRepository.Object);
            _unitOfWork.SetupGet(x => x.CacheRepository).Returns(_cacheRepository.Object);

            var context = new DefaultHttpContext();

            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _httpContextAccessor.Setup(x => x.HttpContext).Returns(context);

            _handler = new RefreshTokenCommandHandler(
                _jwtService.Object,
                _unitOfWork.Object,
                _httpContextAccessor.Object,
                _mapper.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldThrowBadRequestException_WhenTokenIsInvalid()
        {
            // Arrange
            var command = new RefreshTokenCommand("invalidToken");

            _jwtService.Setup(x => x.GetTokenPrincipal(It.IsAny<string>())).Returns<ClaimsPrincipal>(null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, default);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>().WithMessage("Invalid token");
        }

        [Fact]
        public async Task Handle_ShouldThrowBadRequestException_WhenRefreshTokenIsExpired()
        {
            // Arrange
            var command = new RefreshTokenCommand("validToken");
            var identityUser = new RefreshTokenModel { email = "test@test.com", id = 1, refreshToken = "expiredRefreshToken", refreshTokenExpiryTime = DateTime.UtcNow.AddHours(-1) };

            _jwtService.Setup(x => x.GetTokenPrincipal(It.IsAny<string>())).Returns(new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "test@test.com")
            })));

            _userRepository.Setup(x => x.GetTokenModel(It.IsAny<string>())).ReturnsAsync(identityUser);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, default);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>().WithMessage("Invalid token");
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFoundException_WhenUserNotFound()
        {
            // Arrange
            var command = new RefreshTokenCommand("validToken");
            var identityUser = new RefreshTokenModel { email = "test@test.com", id = 1, refreshToken = "validRefreshToken", refreshTokenExpiryTime = DateTime.UtcNow.AddHours(1) };

            _jwtService.Setup(x => x.GetTokenPrincipal(It.IsAny<string>())).Returns(new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "test@test.com")
            })));

            _userRepository.Setup(x => x.GetTokenModel(It.IsAny<string>())).ReturnsAsync(identityUser);
            _cacheRepository.Setup(x => x.GetCacheData<User>(It.IsAny<string>())).ReturnsAsync((User)null);
            _userRepository.Setup(x => x.GetUserById(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, default);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>().WithMessage("User not found");
        }

    }
}
