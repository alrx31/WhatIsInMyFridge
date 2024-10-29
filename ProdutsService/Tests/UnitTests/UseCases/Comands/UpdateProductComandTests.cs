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
    public class UpdateProductComandTests
    {
        private readonly Mock<IProductRepository> _repository;
        private readonly Mock<IMapper> _mapper;

        private readonly UpdateProductComandHandler _handler;

        public UpdateProductComandTests()
        {
            _repository = new Mock<IProductRepository>();
            _mapper = new Mock<IMapper>();

            _handler = new UpdateProductComandHandler(_repository.Object, _mapper.Object);
        }

        [Fact]
        public async Task UpdateProductComand_Success_ShouldUpdateProduct()
        {
            var faker = new Faker();

            var product = new Product
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
                PricePerKilo = faker.Random.Decimal(1, 100),
                ExpirationTime = new TimeSpan(),
            };

            var comand = new UpdateProductComand(
                product.Id,
                new AddProductDTO
                {
                    Name = faker.Commerce.ProductName(),
                    PricePerKilo = faker.Random.Decimal(1, 100),
                    ExpirationTime = new TimeSpan(),
                }
            );

            _repository.Setup(x => x.GetByIdAsync(product.Id, It.IsAny<CancellationToken>())).ReturnsAsync(product);

            // Act

            await _handler.Handle(comand, CancellationToken.None);

            // Assert

            _repository.Verify(x => x.UpdateAsync(product, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateProductComand_Fail_WhenProductNotExist()
        {
            var faker = new Faker();

            var product = new Product
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
                PricePerKilo = faker.Random.Decimal(1, 100),
                ExpirationTime = new TimeSpan(),
            };

            var comand = new UpdateProductComand(
                product.Id,
                new AddProductDTO
                {
                    Name = faker.Commerce.ProductName(),
                    PricePerKilo = faker.Random.Decimal(1, 100),
                    ExpirationTime = new TimeSpan(),
                }
            );

            // Act

            Func<Task> act = async () => await _handler.Handle(comand, CancellationToken.None);

            // Assert

            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
