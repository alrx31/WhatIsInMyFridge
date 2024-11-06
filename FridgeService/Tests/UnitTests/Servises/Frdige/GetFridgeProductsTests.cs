using AutoMapper;
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
    public class GetFridgeProductsTests
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

        public GetFridgeProductsTests()
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
        public async Task GetFridgeProducts_Success_ShouldReturnFridgeProducts()
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
            var productsInFridge = new List<ProductFridgeModel>
            {
                new ProductFridgeModel
                {
                    id = faker.Random.Number(1, 100),
                    productId = faker.Random.Number(0, 100000000).ToString(),
                    count = faker.Random.Number(0, 100),
                    fridgeId = fridge.id,
                    Fridge = fridge,
                    addTime = DateTime.UtcNow
                },
                new ProductFridgeModel
                {
                    id = faker.Random.Number(1, 100),
                    productId = faker.Random.Number(0, 100000000).ToString(),
                    count = faker.Random.Number(0, 100),
                    fridgeId = fridge.id,
                    Fridge = fridge,
                    addTime = DateTime.UtcNow
                }
            };
            var ids = productsInFridge.Select(x => x.productId).ToList();

            var products = new List<Product>
            {
                new Product
                {
                    Id = productsInFridge[0].productId,
                    Count = productsInFridge[0].count,
                    ExpirationTime = faker.Date.Future() - DateTime.UtcNow,
                    Name = faker.Commerce.ProductName(),
                    PricePerKilo = faker.Random.Number(1, 100),
                },
                new Product
                {
                    Id = productsInFridge[1].productId,
                    Count = productsInFridge[0].count,
                    ExpirationTime = faker.Date.Future() - DateTime.UtcNow,
                    Name = faker.Commerce.ProductName(),
                    PricePerKilo = faker.Random.Number(1, 100),
                }
            };

            _fridgeRepository.Setup(x => x.GetProductsFromFridge(fridge.id)).ReturnsAsync(productsInFridge);
            _productsgRPCService.Setup(x => x.GetProducts(ids)).ReturnsAsync(products);

            // Act

            var result = await _handler.GetFridgeProducts(fridge.id);

            // Assert

            result.Should().BeEquivalentTo(products);
        }
    }
}
