using Application.UseCases.Comands;
using Application.UseCases.Handlers.Comands;
using Bogus;
using Domain.Repository;
using Moq;

namespace Tests.UnitTests.UseCases.Commands
{
    public class UserLogoutComandTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<ICacheRepository> _cacheRepository;

        private readonly UserLogoutComandHandler _handler;

        public UserLogoutComandTests()
        {
            _userRepository = new Mock<IUserRepository>();
            _cacheRepository = new Mock<ICacheRepository>();

            _unitOfWork = new Mock<IUnitOfWork>();

            _unitOfWork.SetupGet(x => x.UserRepository).Returns(_userRepository.Object);
            _unitOfWork.SetupGet(x => x.CacheRepository).Returns(_cacheRepository.Object);

            _handler = new UserLogoutComandHandler(
                _unitOfWork.Object
            );
        }


        [Fact]
        public async Task Handle_Success_WhenUserExist()
        {
            var faker = new Faker();

            var command = new UserLogoutCommand
            {
                UserId = faker.Random.Int()
            };

            await _handler.Handle(command, new CancellationToken());

            _userRepository.Verify(x => x.CanselRefreshToken(command.UserId), Times.Once);
            _unitOfWork.Verify(x => x.CompleteAsync(), Times.Once);

        }

    }

}
