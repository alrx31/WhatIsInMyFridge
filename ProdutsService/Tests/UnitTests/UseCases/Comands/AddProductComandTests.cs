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
    public class AddProductComandTests
    {
        private readonly Mock<IProductRepository> _productRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly AddProductComandHandler _handler;

        public AddProductComandTests()
        {
            _productRepository = new Mock<IProductRepository>();
            _mapper = new Mock<IMapper>();

            _handler = new AddProductComandHandler(_productRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task AddProductComand_Success_ShouldAddProduct()
        {
            var faker = new Faker();
            var comand = new AddProductComand(new AddProductDTO
            {
                Name = faker.Commerce.ProductName(),
                PricePerKilo = faker.Random.Decimal(1, 100),
                ExpirationTime = new TimeSpan()
            });

            // Act

            await _handler.Handle(comand, CancellationToken.None);

            // Assert

            _productRepository.Verify(x => x.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddProductComand_Fail_WhenProductsExist()
        {
            var faker = new Faker();

            var product = new Product
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
                PricePerKilo = faker.Random.Decimal(1, 100),
                ExpirationTime = new TimeSpan()
            };

            var comand = new AddProductComand(new AddProductDTO { Name = product.Name });

            _productRepository.Setup(x => x.GetProductByName(product.Name, CancellationToken.None)).ReturnsAsync(product);

            // Act

            Func<Task> act = async () => await _handler.Handle(comand, CancellationToken.None);

            // Assert

            await act.Should().ThrowAsync<AlreadyExistsException>();
        }
    }
}
