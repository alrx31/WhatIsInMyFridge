using AutoMapper;
using AutoMapper.Configuration.Annotations;
using BLL.Exceptions;
using BLL.Services;
using Bogus;
using DAL.Entities;
using DAL.Interfaces;
using DAL.IRepositories;
using DAL.Persistanse.Hubs;
using DAL.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.UnitTests.Servises.Frdige
{
    public class AddUserToFridgeTests
    {
        private readonly Mock<IFridgeRepository> _fridgeRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IgRPCService> _grpcService;
        private readonly Mock<IProductsgRPCService> _productsgRPCService;
        private readonly Mock<IKafkaProducer> _kafkaProducer;
        private readonly Mock<IHubContext<NotificationHub>> _notificationHub;
        private readonly Mock<ILogger<FridgeService>> _logger;


        private readonly IFridgeService _handler;

        public AddUserToFridgeTests()
        {
            _mapper = new Mock<IMapper>();
            _fridgeRepository = new Mock<IFridgeRepository>();

            _unitOfWork = new Mock<IUnitOfWork>();

            _unitOfWork.SetupGet(x => x.FridgeRepository).Returns(_fridgeRepository.Object);

            _grpcService = new Mock<IgRPCService>();

            _kafkaProducer = new Mock<IKafkaProducer>();

            _notificationHub = new Mock<IHubContext<NotificationHub>>();

            _productsgRPCService = new Mock<IProductsgRPCService>();

            _logger = new Mock<ILogger<FridgeService>>();

            _handler = new FridgeService(
                _fridgeRepository.Object,
                _mapper.Object,
                _unitOfWork.Object,
                _grpcService.Object,
                _productsgRPCService.Object,
                _kafkaProducer.Object,
                _logger.Object,
                _notificationHub.Object
            );
        }

        [Fact]
        public async Task AddUserToFridge_Success_ShouldAddUserToFridge()
        {
            var faker = new Faker();
            var fridge = new Fridge
            {
                id = faker.Random.Number(1, 100),
                name = faker.Person.FirstName,
                model = faker.Random.Number(0, 1000000000).ToString(),
                serial = faker.Random.Number(0, 100000000).ToString(),
                boughtDate = DateTime.UtcNow,
                boxNumber = faker.Random.Number(0, 5)
            };
            var user = new User
            {
                id = faker.Random.Number(1, 100),
                name = faker.Person.FirstName,
                email = faker.Person.Email,
                login = faker.Person.UserName,
                password = faker.Random.Number(0, 100000000).ToString(),
                isAdmin = false
            };
            var ModelsUserInFridge = new List<int>();

            _grpcService.Setup(x => x.CheckUserExist(user.id)).ReturnsAsync(true);
            
            _fridgeRepository.Setup(x => x.GetFridgeBySerialAndBoxNumber(fridge.serial,fridge.boxNumber)).ReturnsAsync(fridge);

            _fridgeRepository.Setup(x => x.GetUsersFromFridge(fridge.id)).ReturnsAsync(ModelsUserInFridge);

            // Act

            await _handler.AddUserToFridge(
                fridge.serial,
                fridge.boxNumber,
                user.id
            );

            // Assert

            _fridgeRepository.Verify(x => x.AddUserToFridge(It.IsAny<UserFridge>()), Times.Once);
            
            _unitOfWork.Verify(x => x.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task AddUserToFrdige_Fail_WhenUserNotFound()
        {
            var faker = new Faker();
            var user = new User
            {
                id = faker.Random.Number(1, 100),
                name = faker.Person.FirstName,
                email = faker.Person.Email,
                login = faker.Person.UserName,
                password = faker.Random.Number(0, 100000000).ToString(),
                isAdmin = false
            };

            _grpcService.Setup(x => x.CheckUserExist(user.id)).ReturnsAsync(false);
            // Act

            Func<Task> act = async () => await _handler.AddUserToFridge(
                faker.Random.String(16),
                faker.Random.Number(1,5),
                user.id
            );

            // Assert

            act.Should().ThrowAsync<NotFoundException>().WithMessage("User not found");
        }

        [Fact]
        public async Task AddUserToFridge_Fail_WhenFrdigeNotFound()
        {
            var faker = new Faker();
            var fridge = new Fridge
            {
                id = faker.Random.Number(1, 100),
                name = faker.Person.FirstName,
                model = faker.Random.Number(0, 1000000000).ToString(),
                serial = faker.Random.Number(0, 100000000).ToString(),
                boughtDate = DateTime.UtcNow,
                boxNumber = faker.Random.Number(0, 5)
            };
            var user = new User
            {
                id = faker.Random.Number(1, 100),
                name = faker.Person.FirstName,
                email = faker.Person.Email,
                login = faker.Person.UserName,
                password = faker.Random.Number(0, 100000000).ToString(),
                isAdmin = false
            };

            _grpcService.Setup(x => x.CheckUserExist(user.id)).ReturnsAsync(true);



            // Act

            Func<Task> act = async () => await _handler.AddUserToFridge(
                fridge.serial,
                fridge.boxNumber,
                user.id
            );

            // Assert
            
            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Fridge not found");
        }

        [Fact]
        public async Task AddUserToFridge_Fail_WhenUserAlreadyInFridge()
        {
            var faker = new Faker();
            var fridge = new Fridge
            {
                id = faker.Random.Number(1, 100),
                name = faker.Person.FirstName,
                model = faker.Random.Number(0, 1000000000).ToString(),
                serial = faker.Random.Number(0, 100000000).ToString(),
                boughtDate = DateTime.UtcNow,
                boxNumber = faker.Random.Number(0, 5)
            };
            var user = new User
            {
                id = faker.Random.Number(1, 100),
                name = faker.Person.FirstName,
                email = faker.Person.Email,
                login = faker.Person.UserName,
                password = faker.Random.Number(0, 100000000).ToString(),
                isAdmin = false
            };
            var ModelsUserInFridge = new List<int>()
            {
                user.id
            };

            _grpcService.Setup(x => x.CheckUserExist(user.id)).ReturnsAsync(true);

            _fridgeRepository.Setup(x => x.GetFridgeBySerialAndBoxNumber(fridge.serial, fridge.boxNumber)).ReturnsAsync(fridge);

            _fridgeRepository.Setup(x => x.GetUsersFromFridge(fridge.id)).ReturnsAsync(ModelsUserInFridge);

            // Act

            Func<Task> act = async () => await _handler.AddUserToFridge(
                fridge.serial,
                fridge.boxNumber,
                user.id
            );

            // Assert

            await act.Should().ThrowAsync<BadRequestException>().WithMessage("User already use this fridge");
        }
    }
}
