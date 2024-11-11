using AutoMapper;
using BLL.DTO;
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
    public class AddProductsToFridgeTests
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

        public AddProductsToFridgeTests()
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
        public async Task AddProductsToFridgeTest()
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
            var existProducts = new List<ProductFridgeModel>();


            var products = new List<ProductInfoModel>
            {
                new ProductInfoModel
                {
                    ProductId = "test",
                    Count = 1
                },
                new ProductInfoModel
                {
                    ProductId = "test2",
                    Count = 2
                }
            };

            _fridgeRepository.Setup(x => x.GetFridge(fridge.id)).ReturnsAsync(fridge);
            _fridgeRepository.Setup(x => x.GetProductsFromFridge(fridge.id)).ReturnsAsync(existProducts);

            // Act

            await _handler.AddProductsToFridge(fridge.id, products);

            // Assert

            _fridgeRepository.Verify(x => x.AddProductsToFridge(It.IsAny<List<ProductFridgeModel>>()), Times.Once);
            _fridgeRepository.Verify(x => x.GetFridge(fridge.id), Times.Once);
            _fridgeRepository.Verify(x => x.GetProductsFromFridge(fridge.id), Times.Once);
            _unitOfWork.Verify(x => x.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task AddProductToFrdige_Fail_WhenFridgeNotFound()
        {
            var products = new List<ProductInfoModel>
            {
                new ProductInfoModel
                {
                    ProductId = "test",
                    Count = 1
                },
                new ProductInfoModel
                {
                    ProductId = "test2",
                    Count = 2
                }
            };

            // Act

            Func<Task> act = async () => await _handler.AddProductsToFridge(1, products);

            // Assert

            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
