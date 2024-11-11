using Bogus;
using Domain.Repository;
using FluentAssertions;
using Grpc.Core;
using Moq;

namespace Tests.UnitTests
{
    public class GRPCTests
    {
        private readonly Mock<IProductRepository> _repository;
        private readonly Infrastructure.Persistance.ProductgRPCService _productsService;
        private readonly ServerCallContext _serverCallContext;

        public GRPCTests()
        {
            _repository = new Mock<IProductRepository>();

            _productsService = new Infrastructure.Persistance.ProductgRPCService(_repository.Object);

            _serverCallContext = new Mock<ServerCallContext>().Object;
        }

        [Fact]
        public async Task GRPCTests_Success_ShouldReturnProducts()
        {
            var faker = new Faker();
            var products = new List<Domain.Entities.Product>
            {
                new Domain.Entities.Product
                {
                    Id = faker.Random.Guid().ToString(),
                    Name = faker.Commerce.ProductName(),
                    PricePerKilo = faker.Random.Decimal(),
                    ExpirationTime = new TimeSpan()
                }
            };

            _repository.Setup(x => x.GetProductRange(It.IsAny < List<string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            // Act

            var response = await _productsService.GetProducts(new Infrastructure.Persistanse.Protos.ProductsIds(), _serverCallContext);

            // Asesrt

            response.Should().NotBeNull();
        }
    }
}
