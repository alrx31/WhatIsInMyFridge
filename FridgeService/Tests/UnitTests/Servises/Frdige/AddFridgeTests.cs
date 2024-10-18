using AutoMapper;
using BLL.DTO;
using BLL.Services;
using Bogus;
using Castle.Core.Logging;
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
    public class AddFridgeTests
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

        public AddFridgeTests()
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
        public async Task AddFridge_Success_ShouldAddFridgeToDb() {
            var faker = new Faker();

            var fridgeAddModel = new FridgeAddDTO
            {
                Name = faker.Person.FirstName,
                Model = faker.Random.Number(0, 1000000000).ToString(),
                Serial = faker.Random.Number(0, 100000000).ToString(),
                BoughtDate = DateTime.UtcNow,
                BoxNumber = faker.Random.Number(0, 10)
            };

            _mapper.Setup(x => x.Map<Fridge>(fridgeAddModel)).Returns(new Fridge());

            _unitOfWork.Setup(x => x.FridgeRepository.AddFridge(It.IsAny<Fridge>())).Returns(Task.CompletedTask);

            _unitOfWork.Setup(x => x.CompleteAsync())
                .Returns(Task.CompletedTask);

            // Act

            var result = _handler.AddFridge(fridgeAddModel);

            // Assert

            result.Should().Be(Task.CompletedTask);
            _unitOfWork.Verify(x => x.FridgeRepository.AddFridge(It.IsAny<Fridge>()), Times.Once);
            _unitOfWork.Verify(x => x.CompleteAsync(), Times.Once);
        }



    }
}
