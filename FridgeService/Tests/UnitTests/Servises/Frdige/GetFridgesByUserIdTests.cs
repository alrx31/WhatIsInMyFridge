using AutoMapper;
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
using Xunit.Sdk;

namespace Tests.UnitTests.Servises.Frdige
{
    public class GetFridgesByUserIdTests
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

        public GetFridgesByUserIdTests()
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
        public async Task GetFridgesByUserId_Success_ShouldReturnUserFridges()
        {
            var faker = new Faker();

            var user = new User
            {
                id = faker.Random.Number(1, 1000),
                name = faker.Random.String(10),
                login = faker.Person.FirstName,
                email = faker.Internet.Email(),
                password = faker.Random.String(10),
                isAdmin = false,
            };

            var fridges = new List<Fridge>
            {
                new Fridge
                {
                    id = faker.Random.Number(1,10000),
                    name = faker.Person.FirstName,
                    model = faker.Random.Number(0, 1000000000).ToString(),
                    serial = faker.Random.Number(0, 100000000).ToString(),
                    boughtDate = DateTime.UtcNow,
                    boxNumber = faker.Random.Number(0, 10)
                },
                new Fridge
                {
                    id = faker.Random.Number(1,10000),
                    name = faker.Person.FirstName,
                    model = faker.Random.Number(0, 1000000000).ToString(),
                    serial = faker.Random.Number(0, 100000000).ToString(),
                    boughtDate = DateTime.UtcNow,
                    boxNumber = faker.Random.Number(0, 10)
                },
            };

            _grpcService.Setup(x => x.CheckUserExist(user.id)).ReturnsAsync(true);

            _fridgeRepository.Setup(x => x.GetFridgeByUserId(user.id)).ReturnsAsync(fridges);

            // Act

            var result = await _handler.GetFridgesByUserId(user.id);

            // Assert

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(fridges);
        }

        [Fact]
        public async Task GetFridgeByUserId_Fail_UserNotFound()
        {
            var faker = new Faker();
            var userId = faker.Random.Number(1, 1000);

            // Act
            Func<Task<List<Fridge>>> act = async () => await _handler.GetFridgesByUserId(userId);

            // Assert

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("User not found");
        }

        [Fact]
        public async Task GetFridgeByUserId_Fail_FridgesNotFound()
        {
            var faker = new Faker();

            var user = new User
            {
                id = faker.Random.Number(1, 1000),
                name = faker.Random.String(10),
                login = faker.Person.FirstName,
                email = faker.Internet.Email(),
                password = faker.Random.String(10),
                isAdmin = false,
            };

            _grpcService.Setup(x=>x.CheckUserExist(user.id)).ReturnsAsync(true);

            // Act

            Func<Task<List<Fridge>>> act = async () => await _handler.GetFridgesByUserId(user.id);

            // Assert

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Fridges not found");
        }
    }
}
