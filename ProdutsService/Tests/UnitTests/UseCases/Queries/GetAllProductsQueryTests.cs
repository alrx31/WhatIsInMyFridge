using Application.UseCases.Queries;
using Application.UseCases.QueriesHandlers;
using Bogus;
using Domain.Entities;
using Domain.Repository;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.UseCases.Queries
{
    public class GetAllProductsQueryTests
    {
        private readonly Mock<IProductRepository> _productRepository;

        private readonly GetAllProductsQueryHandler _handler;

        public GetAllProductsQueryTests()
        {
            _productRepository = new Mock<IProductRepository>();

            _handler = new GetAllProductsQueryHandler(
                _productRepository.Object
            );
        }

        [Fact]
        public async Task GetAllProductsQuery_Success_ShouldReturnProducts()
        {
            var faker = new Faker();
            var products = new List<Product>
            {
                new Product
                {
                    Id = faker.Random.Int().ToString(),
                    Name = faker.Lorem.Word(),
                    PricePerKilo = faker.Random.Decimal(),
                    ExpirationTime = new TimeSpan(faker.Random.Int(), faker.Random.Int(), faker.Random.Int()),
                },
                new Product
                {
                    Id = faker.Random.Int().ToString(),
                    Name = faker.Lorem.Word(),
                    PricePerKilo = faker.Random.Decimal(),
                    ExpirationTime = new TimeSpan(),
                }
            };

            var query = new GetAllProductsQuery()
            {
                Page = faker.Random.Int(),
                Count = faker.Random.Int()
            };

            _productRepository.Setup(x => x.GetAllPaginationAsync(query.Page, query.Count, new CancellationToken())).ReturnsAsync(products);

            // Act

            var result = await _handler.Handle(query, new CancellationToken());

            // Assert

            result.Should().BeEquivalentTo(products);
        }
    }
}
