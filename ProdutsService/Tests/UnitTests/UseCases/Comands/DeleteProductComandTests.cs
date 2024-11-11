using Application.Exceptions;
using Application.UseCases.Comands;
using Application.UseCases.ComandsHandlers;
using Bogus;
using Domain.Entities;
using Domain.Repository;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.UseCases.Comands
{
    public class DeleteProductComandTests
    {
        private readonly Mock<IProductRepository> _repository;

        private readonly DeleteProductComandHandler _handler;

        public DeleteProductComandTests()
        {
            _repository = new Mock<IProductRepository>();

            _handler = new DeleteProductComandHandler(_repository.Object);
        }

        [Fact]
        public async Task DeleteProductComand_Success_ShouldDeleteProduct()
        {
            var faker = new Faker();

            var product = new Product
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
                PricePerKilo = faker.Random.Decimal(1, 100),
                ExpirationTime = new TimeSpan(),
            };

            _repository.Setup(x => x.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act

            await _handler.Handle(new DeleteProductComand(product.Id), CancellationToken.None);

            // Assert

            _repository.Verify(x => x.DeleteAsync(product.Id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteProductComand_Fail_ShouldThrowNotFoundException()
        {
            var faker = new Faker();

            var product = new Product
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
                PricePerKilo = faker.Random.Decimal(1, 100),
                ExpirationTime = new TimeSpan(),
            };

            _repository.Setup(x => x.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);

            // Act

            Func<Task> act = async () => await _handler.Handle(new DeleteProductComand(product.Id), CancellationToken.None);

            // Assert

            await act.Should().ThrowAsync<NotFoundException>();
            _repository.Verify(x => x.DeleteAsync(product.Id, It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
