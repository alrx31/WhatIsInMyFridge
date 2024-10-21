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
    public class UpdateFridgeTests
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

        public UpdateFridgeTests()
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
        public async Task UpdateFridge_Success_ShouldUpdateFridge(){
            var faker = new Faker();
            var existingFridge = new Fridge
            {
                id = faker.Random.Number(1, 100),
                name = faker.Person.FirstName,
                model = faker.Random.Number(0, 1000000000).ToString(),
                serial = faker.Random.Number(0, 100000000).ToString(),
                boughtDate = DateTime.UtcNow,
                boxNumber = faker.Random.Number(0, 10)
            };

            var fridgeDto = new FridgeAddDTO
            {
                Name = faker.Person.FirstName,
                Model = faker.Random.Number(0, 1000000000).ToString(),
                Serial = faker.Random.Number(0, 100000000).ToString(),
                BoughtDate = DateTime.UtcNow,
                BoxNumber = faker.Random.Number(0, 10)
            };

            _fridgeRepository.Setup(x => x.GetFridge(existingFridge.id)).ReturnsAsync(existingFridge);
            _mapper.Setup(x => x.Map(fridgeDto,existingFridge)).Returns(existingFridge);
            _fridgeRepository.Setup(x => x.UpdateFridge(existingFridge)).ReturnsAsync(existingFridge);

            // Act

            var result = await _handler.UpdateFridge(fridgeDto, existingFridge.id);

            // Assert

            result.Should().BeEquivalentTo(existingFridge);
        }

        [Fact]
        public async Task UpdateFridge_Fail_FridgeNotFound()
        {
            var faker = new Faker();
            var fridgeId = faker.Random.Int();

            // Act

            Func<Task<Fridge>> act = async () => await _handler.UpdateFridge(new FridgeAddDTO(), fridgeId);

            // Assert

            act.Should().ThrowAsync<NotFoundException>().WithMessage("Fridge not found");

        }
    }
}