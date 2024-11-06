using AutoMapper;
using AutoMapper.Configuration.Annotations;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Services;
using Bogus;
using Castle.Core.Logging;
using DAL.Entities;
using DAL.Interfaces;
using DAL.IRepositories;
using DAL.Persistanse.Hubs;
using DAL.Persistanse.Protos;
using DAL.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.UnitTests.Servises.Frdige
{
    public class GetFridgeTests
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

        public GetFridgeTests()
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
        public async Task AddFridge_Success_ShoudReturnFridgeById()
        {
            var faker = new Faker();

            var fridgeId = faker.Random.Number(1, 100);

            var fridge = new Fridge
            {
                id = fridgeId,
                name = faker.Person.FirstName,
                model = faker.Random.Number(0, 1000000000).ToString(),
                serial = faker.Random.Number(0, 100000000).ToString(),
                boughtDate = DateTime.UtcNow,
                boxNumber = faker.Random.Number(0, 10)
            };
            
            _fridgeRepository.Setup(x => x.GetFridge(fridgeId)).ReturnsAsync(fridge);

            // Act

            var result = await _handler.GetFridge(fridgeId);

            // Assert

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(fridge);
            result.id.Should().Be(fridgeId);
        }

        [Fact]
        public async Task GetFridge_Fail_WhenFridgeNotFound()
        {
            var faker = new Faker();

            var fridgeId = faker.Random.Number(10, 100);

            // Act

            Func<Task> act = async () =>  await _handler.GetFridge(fridgeId);

            // Assert

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Fridge not found");
        }
    }
}
