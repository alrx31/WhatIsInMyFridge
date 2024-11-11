using Application.DTO;
using Application.Exceptions;
using Application.UseCases.Comands;
using Application.UseCases.ComandsHandlers;
using AutoMapper;
using Bogus;
using Domain.Entities;
using Domain.Repository;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.UseCases.Comands
{
    public class UpdateListComandTests
    {
        private readonly Mock<IListRepository> _listRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly UpdateListComandHandler _handler;

        public UpdateListComandTests()
        {
            _listRepository = new Mock<IListRepository>();
            _mapper = new Mock<IMapper>();

            _handler = new UpdateListComandHandler(_listRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task UpdateListComand_Success_ShouldUpdateList()
        {
            var faker = new Faker();

            var list = new ProductsList
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
            };

            var product = new Product
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
                PricePerKilo = faker.Random.Decimal(1, 100),
                ExpirationTime = new TimeSpan(),
            };

            var comand = new UpdateListComand(
                list.Id,
                new AddListDTO
                {
                    Name = faker.Commerce.ProductName(),
                    Weight = faker.Random.Int(1, 100),
                    FridgeId = faker.Random.Int(1, 100),
                    BoxNumber = faker.Random.Int(1, 100),
                    Price = faker.Random.Decimal(1, 100)
                },
                new List<Product>()
            );

            _listRepository.Setup(x => x.GetByIdAsync(list.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(list);

            // Act

            await _handler.Handle(comand, CancellationToken.None);

            // Assert

            _mapper.Verify(x => x.Map(comand, list), Times.Once);
            _listRepository.Verify(x => x.UpdateAsync(list, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateListComand_Fail_WhenListNotFound()
        {
            var faker = new Faker();

            var list = new ProductsList
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
            };

            var product = new Product
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
                PricePerKilo = faker.Random.Decimal(1, 100),
                ExpirationTime = new TimeSpan(),
            };

            var comand = new UpdateListComand(
                list.Id,
                new AddListDTO
                {
                    Name = faker.Commerce.ProductName(),
                    Weight = faker.Random.Int(1, 100),
                    FridgeId = faker.Random.Int(1, 100),
                    BoxNumber = faker.Random.Int(1, 100),
                    Price = faker.Random.Decimal(1, 100)
                },
                new List<Product>()
            );

            _listRepository.Setup(x => x.GetByIdAsync(list.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductsList)null);

            // Act

            Func<Task> act = async () => await _handler.Handle(comand, CancellationToken.None);

            // Assert

            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
