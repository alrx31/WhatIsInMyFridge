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

namespace Tests.UnitTests.Servises.Frdige
{
    public class GetFridgeUsers
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

        public GetFridgeUsers()
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
        public async Task GetFridgeUsers_Success_ShouldReturnsUsers()
        {
            var faker = new Faker();
            var fridge = new Fridge
            {
                id = faker.Random.Number(1, 100),
                name = faker.Person.FirstName,
                model = faker.Random.Number(0, 1000000000).ToString(),
                serial = faker.Random.Number(0, 100000000).ToString(),
                boughtDate = DateTime.UtcNow,
                boxNumber = faker.Random.Number(0, 10)
            };
            var users = new List<User>
            {
                new User
                {
                    id = faker.Random.Number(1, 100),
                    name = faker.Person.FirstName,
                    email = faker.Person.Email,
                    login = faker.Person.UserName,
                    password = faker.Random.Number(0, 100000000).ToString(),
                    isAdmin = false
                }
            };
            var ids = users.Select(x => x.id).ToList();

            _fridgeRepository.Setup(x => x.GetFridge(fridge.id)).ReturnsAsync(fridge);
            _fridgeRepository.Setup(x => x.GetUsersFromFridge(fridge.id)).ReturnsAsync(ids);
            _grpcService.Setup(x => x.GetUsers(ids)).ReturnsAsync(users);

            // Act

            var result = await _handler.GetFridgeUsers(fridge.id);

            // Assert

            result.Should().BeEquivalentTo(users);
        }

        [Fact]
        public async Task GetFridgeUsers_Fail_WhenFridgeNotFound()
        {
            var faker = new Faker();
            var fridgeId = faker.Random.Int();

            // Act

            Func<Task> act = async () => await _handler.GetFridgeUsers(fridgeId);

            // Assert

            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
