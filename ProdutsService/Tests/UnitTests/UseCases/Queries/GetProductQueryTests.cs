using Application.UseCases.Queries;
using Application.UseCases.QueriesHandlers;
using Bogus;
using Domain.Entities;
using Domain.Repository;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.UseCases.Queries
{
    public class GetProductQueryTests
    {
        private readonly Mock<IProductRepository> _repository;

        private readonly GetProductQueryHandler _handler;

        public GetProductQueryTests()
        {
            _repository = new Mock<IProductRepository>();

            _handler = new GetProductQueryHandler(
                _repository.Object
            );
        }

        [Fact]
        public async Task GetProductQuery_Success_ShouldReturnProduct()
        {
            var faker = new Faker();
            var product = new Product
            {
                Id = faker.Random.Guid().ToString(),
                Name = faker.Random.String(),
                PricePerKilo = faker.Random.Decimal(),
                ExpirationTime = new TimeSpan()
            };

            _repository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            var request = new GetProductQuery
            (
                product.Id
            );

            // Act

            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert

            result.Should().BeEquivalentTo(product);
        }
    }
}
