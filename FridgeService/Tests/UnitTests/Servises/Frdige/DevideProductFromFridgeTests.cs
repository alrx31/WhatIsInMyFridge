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
    public class DevideProductFromFridgeTests
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

        public DevideProductFromFridgeTests()
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
        public async Task DevideProductFromFridge_Success_ShouldDevideProduct()
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
            var product = new ProductFridgeModel
            {
                id = faker.Random.Number(1, 100),
                productId = faker.Random.Number(0, 100000000).ToString(),
                count = faker.Random.Number(10, 100),
                fridgeId = fridge.id,
                Fridge = fridge,
                addTime = DateTime.UtcNow
            };

            _fridgeRepository.Setup(x => x.GetFridge(fridge.id)).ReturnsAsync(fridge);
            _fridgeRepository.Setup(x => x.GetProductFromFridge(fridge.id, product.productId))
                .ReturnsAsync(product);

            // Act
            
            await _handler.DevideProductFromFridge(fridge.id,faker.Random.Number(1,9), product.productId);

            // Assert

            _fridgeRepository.Verify(x => x.UpdateProductInFridge(product), Times.Once);
            _unitOfWork.Verify(x => x.CompleteAsync(), Times.Once);

        }

        [Fact]
        public async Task DevideProductFromFridge_Fail_WhenFridgeNotFound() {
            var faker = new Faker();
            var fridgeId = faker.Random.Number(1, 100);

            // Act

            Func<Task> act = async () => await _handler.DevideProductFromFridge(fridgeId, faker.Random.Number(1, 9), faker.Random.Number(0, 100000000).ToString());

            // Assert

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Fridge not found");
        }

        [Fact]
        public async Task DevideProudctFromFridge_Fail_WhenProductNotFound()
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
            _fridgeRepository.Setup(x => x.GetFridge(fridge.id)).ReturnsAsync(fridge);

            // Act

            Func<Task> act = async () => await _handler.DevideProductFromFridge(fridge.id, faker.Random.Number(1, 9), faker.Random.Number(0, 100000000).ToString());

            // Assert

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Product in fridge not found");
        }

        [Fact]
        public async Task DevideProductFromFridge_Fail_WhenInvalidCount()
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
            var product = new ProductFridgeModel
            {
                id = faker.Random.Number(1, 100),
                productId = faker.Random.Number(0, 100000000).ToString(),
                count = faker.Random.Number(10, 100),
                fridgeId = fridge.id,
                Fridge = fridge,
                addTime = DateTime.UtcNow
            };

            _fridgeRepository.Setup(x => x.GetFridge(fridge.id)).ReturnsAsync(fridge);
            _fridgeRepository.Setup(x => x.GetProductFromFridge(fridge.id, product.productId))
                .ReturnsAsync(product);

            // Act

            Func<Task> act = async () => await _handler.DevideProductFromFridge(fridge.id, product.count + 1, product.productId);

            // Assert

            await act.Should().ThrowAsync<BadRequestException>().WithMessage("Invalid count of product");
        }
    }
}
