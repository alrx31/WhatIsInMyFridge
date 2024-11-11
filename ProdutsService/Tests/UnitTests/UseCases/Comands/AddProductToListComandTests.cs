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
    public class AddProductToListComandTests
    {
        private readonly Mock<IListManageRepository> _listManageRepository;
        private readonly Mock<IProductRepository> _productRepository;
        private readonly Mock<IListRepository> _listRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly AddProductToListComandHandler _handler;

        public AddProductToListComandTests()
        {
            _listManageRepository = new Mock<IListManageRepository>();
            _productRepository = new Mock<IProductRepository>();
            _listRepository = new Mock<IListRepository>();
            _mapper = new Mock<IMapper>();

            _handler = new AddProductToListComandHandler(_listManageRepository.Object, _productRepository.Object, _listRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task AddProductToListComand_Success_ShouldAddProductToList()
        {
            var faker = new Faker();
            var list = new ProductsList
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
                Weight = faker.Random.Int(1, 100),
                FridgeId = faker.Random.Int(1, 100),
                BoxNumber = faker.Random.Int(1, 100),
                Price = faker.Random.Decimal(1, 100)
            };

            var product = new Product
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
                PricePerKilo = faker.Random.Decimal(1, 100),
                ExpirationTime = new TimeSpan(),
            };

            _listRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(list);
            _productRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            var comand = new AddProductToListComand(list.Id, new AddProductToListDTO
            {
                ProductId = product.Id,
                Weight = faker.Random.Int(1, 100),
                Cost = faker.Random.Decimal(1, 100)
            });

            // Act

            await _handler.Handle(comand, CancellationToken.None);

            // Assert

            _listManageRepository.Verify(x => x.AddAsync(It.IsAny<ProductInList>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddProductToListComandTests_Fail_WhenListNotExist()
        {
            var faker = new Faker();

            var comand = new AddProductToListComand(faker.Random.Int(1, 100).ToString(), new AddProductToListDTO
            {
                ProductId = faker.Random.Int(1, 100).ToString(),
                Weight = faker.Random.Int(1, 100),
                Cost = faker.Random.Decimal(1, 100)
            });

            // Act

            Func<Task> act = async () => await _handler.Handle(comand, CancellationToken.None);

            // Assert

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("List not found");
        }

        [Fact]
        public async Task AddProductToListComand_Fail_WhenProductNotExist()
        {
            var faker = new Faker();
            var list = new ProductsList
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
                Weight = faker.Random.Int(1, 100),
                FridgeId = faker.Random.Int(1, 100),
                BoxNumber = faker.Random.Int(1, 100),
                Price = faker.Random.Decimal(1, 100)
            };

            _listRepository.Setup(x => x.GetByIdAsync(list.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(list);

            var comand = new AddProductToListComand(list.Id, new AddProductToListDTO
            {
                ProductId = faker.Random.Int(1, 100).ToString(),
                Weight = faker.Random.Int(1, 100),
                Cost = faker.Random.Decimal(1, 100)
            });

            // Act

            Func<Task> act = async () => await _handler.Handle(comand, CancellationToken.None);

            // Assert

            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
