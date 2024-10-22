using AutoMapper;
using Domain.Repository;
using FluentAssertions;
using Grpc.Core;
using Infastructure.Persistanse.Protos;
using Moq;

namespace Tests.UnitTests.Repositories
{
    public class GreeterServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GreeterService _greeterService;

        public GreeterServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _greeterService = new GreeterService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        private ServerCallContext CreateMockServerCallContext()
        {
            return new Mock<ServerCallContext>().Object;

        }

        [Fact]
        public async Task GetUsers_ValidIds_ReturnsUsers()
        {
            var userIds = new UsersIds { Ids = { 1, 2, 3 } };
            var userList = new List<Domain.Entities.User>
            {
                new Domain.Entities.User { id = 1, name = "User1" },
                new Domain.Entities.User { id = 2, name = "User2" },
                new Domain.Entities.User { id = 3, name = "User3" }
            };

            _unitOfWorkMock.Setup(x => x.UserRepository.GetUserById(It.IsAny<int>()))
                .ReturnsAsync((int id) => userList.Find(u => u.id == id));

            _mapperMock.Setup(m => m.Map<User>(It.IsAny<Domain.Entities.User>()))
                .Returns((Domain.Entities.User u) => new User { Id = u.id, Name = u.name });

            var response = await _greeterService.GetUsers(userIds, CreateMockServerCallContext());

            response.Should().NotBeNull();
            response.Users.Should().HaveCount(3);
            response.Users.Should().ContainSingle(u => u.Id == 1 && u.Name == "User1");
            response.Users.Should().ContainSingle(u => u.Id == 2 && u.Name == "User2");
            response.Users.Should().ContainSingle(u => u.Id == 3 && u.Name == "User3");

            _unitOfWorkMock.Verify(x => x.UserRepository.GetUserById(It.IsAny<int>()), Times.Exactly(3));
        }

        [Fact]
        public async Task CheckUserExist_UserExists_ReturnsIsExistTrue()
        {
            var userId = 1;
            var request = new UserExistRequest { UserId = userId };
            var user = new Domain.Entities.User { id = userId, name = "User1" };

            _unitOfWorkMock.Setup(x => x.UserRepository.GetUserById(userId))
                .ReturnsAsync(user);

            var response = await _greeterService.CheckUserExist(request, CreateMockServerCallContext());

            response.Should().NotBeNull();
            response.IsExist.Should().BeTrue();
        }

        [Fact]
        public async Task CheckUserExist_UserDoesNotExist_ReturnsIsExistFalse()
        {
            var userId = 2;
            var request = new UserExistRequest { UserId = userId };

            _unitOfWorkMock.Setup(x => x.UserRepository.GetUserById(userId))
                .ReturnsAsync((Domain.Entities.User)null);

            var response = await _greeterService.CheckUserExist(request, CreateMockServerCallContext());

            response.Should().NotBeNull();
            response.IsExist.Should().BeFalse();
        }
    }
}
