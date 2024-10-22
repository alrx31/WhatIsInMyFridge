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
    public class CheckProducts
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

        public CheckProducts()
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
        public async Task CheckProductsInOneFridge_Success_ShouldCheckProducts()
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

            var users = new List<User>
            {
                new User
                {
                    id = faker.Random.Number(1, 100),
                    name = faker.Person.FirstName,
                    email = faker.Person.Email,
                    login = faker.Person.UserName,
                    password = faker.Random.String2(10),
                    isAdmin = false
                }
            };

            var productsInFridge = new List<ProductFridgeModel>
            {
                 new ProductFridgeModel
                {
                    id = faker.Random.Number(1, 100),
                    productId = faker.Random.Number(1, 100).ToString(),
                    count = faker.Random.Number(1, 100),
                    fridgeId = fridge.id,
                    Fridge = fridge,
                    addTime = DateTime.UtcNow
                }
            };

            var products = new List<Product>
            {
                new Product
                {
                    Id = productsInFridge[0].productId,
                    Count = productsInFridge[0].count,
                    ExpirationTime = faker.Date.Future() - DateTime.UtcNow,
                    Name = faker.Commerce.ProductName(),
                    PricePerKilo = faker.Random.Number(1, 100),
                }
            };

            _fridgeRepository.Setup(x => x.GetFridge(fridge.id)).ReturnsAsync(fridge);
            _fridgeRepository.Setup(x => x.GetProductsFromFridge(fridge.id)).ReturnsAsync(productsInFridge);
            
            var ids = productsInFridge.Select(x => x.productId).ToList();
            _productsgRPCService.Setup(x => x.GetProducts(ids)).ReturnsAsync(products);

            var userIds = users.Select(x => x.id).ToList();

            _fridgeRepository.Setup(x => x.GetUsersFromFridge(fridge.id)).ReturnsAsync(userIds);

            _grpcService.Setup(x => x.GetUsers(userIds)).ReturnsAsync(users);

            // Act

            await _handler.CheckProducts(fridge.id);

            // Assert

            _productsgRPCService.Verify(x => x.GetProducts(ids), Times.Once);
            _fridgeRepository.Verify(x => x.GetFridge(fridge.id), Times.AtLeastOnce);
            _fridgeRepository.Verify(x => x.GetProductsFromFridge(fridge.id), Times.Once);
            _fridgeRepository.Verify(x => x.GetUsersFromFridge(fridge.id), Times.Once);
            _grpcService.Verify(x => x.GetUsers(userIds), Times.Once);
        }

        [Fact]
        public async Task ChectProductsInOneFridge_Fail_WhenFridgeNotFound()
        {
            var faker = new Faker();
            var fridgeId = faker.Random.Number(1, 100);

            // Act

            Func<Task> act = async () => await _handler.CheckProducts(fridgeId);

            // Assert

            act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task CheckProductsInAllFridges_Success()
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

            var fridges = new List<Fridge> { fridge };

            var users = new List<User>
            {
                new User
                {
                    id = faker.Random.Number(1, 100),
                    name = faker.Person.FirstName,
                    email = faker.Person.Email,
                    login = faker.Person.UserName,
                    password = faker.Random.String2(10),
                    isAdmin = false
                }
            };

            var productsInFridge = new List<ProductFridgeModel>
            {
                 new ProductFridgeModel
                {
                    id = faker.Random.Number(1, 100),
                    productId = faker.Random.Number(1, 100).ToString(),
                    count = faker.Random.Number(1, 100),
                    fridgeId = fridge.id,
                    Fridge = fridge,
                    addTime = DateTime.UtcNow
                }
            };

            var products = new List<Product>
            {
                new Product
                {
                    Id = productsInFridge[0].productId,
                    Count = productsInFridge[0].count,
                    ExpirationTime = faker.Date.Future() - DateTime.UtcNow,
                    Name = faker.Commerce.ProductName(),
                    PricePerKilo = faker.Random.Number(1, 100),
                }
            };
            _fridgeRepository.Setup(x => x.GetAllFridges()).ReturnsAsync(fridges);
            _fridgeRepository.Setup(x => x.GetFridge(fridge.id)).ReturnsAsync(fridge);
            _fridgeRepository.Setup(x => x.GetProductsFromFridge(fridge.id)).ReturnsAsync(productsInFridge);

            var ids = productsInFridge.Select(x => x.productId).ToList();
            _productsgRPCService.Setup(x => x.GetProducts(ids)).ReturnsAsync(products);

            var userIds = users.Select(x => x.id).ToList();

            _fridgeRepository.Setup(x => x.GetUsersFromFridge(fridge.id)).ReturnsAsync(userIds);

            _grpcService.Setup(x => x.GetUsers(userIds)).ReturnsAsync(users);

            // Act

            await _handler.CheckProducts();

            // Assert

            _productsgRPCService.Verify(x => x.GetProducts(ids), Times.Once);
            _fridgeRepository.Verify(x => x.GetFridge(fridge.id), Times.AtLeastOnce);
            _fridgeRepository.Verify(x => x.GetProductsFromFridge(fridge.id), Times.Once);
            _fridgeRepository.Verify(x => x.GetUsersFromFridge(fridge.id), Times.Once);
            _grpcService.Verify(x => x.GetUsers(userIds), Times.Once);
        }
    }
}
