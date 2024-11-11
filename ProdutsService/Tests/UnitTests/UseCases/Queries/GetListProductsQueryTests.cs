using Application.Exceptions;
using Application.UseCases.Queries;
using Application.UseCases.QueriesHandlers;
using Bogus;
using Domain.Entities;
using Domain.Repository;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.UseCases.Queries
{
    public class GetListProductsQueryTests
    {
        private readonly Mock<IListManageRepository> _listManageRepository;
        private readonly Mock<IListRepository> _listRepository;
        private readonly Mock<IProductRepository> _productRepository;

        private readonly GetListProductsQueryHandler _handler;

        public GetListProductsQueryTests()
        {
            _listManageRepository = new Mock<IListManageRepository>();
            _listRepository = new Mock<IListRepository>();
            _productRepository = new Mock<IProductRepository>();

            _handler = new GetListProductsQueryHandler(
                _listManageRepository.Object,
                _listRepository.Object,
                _productRepository.Object
            );
        }

        [Fact]
        public async Task GetListProductsQuery_Success_ShouldReturnProducts()
        {
            var faker = new Faker();
            var list = new ProductsList
            {
                Id = faker.Random.Guid().ToString(),
                Name = faker.Random.String(),
                FridgeId = faker.Random.Number(1, 10),
                BoxNumber = faker.Random.Number(1, 10),
                Weight = faker.Random.Number(1, 10),
                HowPackeges = faker.Random.Number(1, 10),
                Price = faker.Random.Decimal(),
                CreateData = faker.Date.Past()
            };
            var products = new List<Product>
            {
                new Product
                {
                    Id = faker.Random.Guid().ToString(),
                    Name = faker.Random.String(),
                    PricePerKilo = faker.Random.Decimal(),
                    ExpirationTime = TimeSpan.FromMinutes(faker.Random.Number(1, 60)),
                }
            };
            var prIds = products.Select(products => products.Id).ToList();

            _listRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(list);

            _listManageRepository.Setup(x => x.GetListProducts(list.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(prIds);

            _productRepository.Setup(x => x.GetProductRange(prIds, It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            var query = new GetListProductsQuery
            {
                ListId = list.Id
            };

            // Act

            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert

            result.Should().BeEquivalentTo(products);
        }

        [Fact]
        public async Task GetListProductsQuery_Fail_WhenListNotExist()
        {
            var faker = new Faker();

            var query = new GetListProductsQuery
            {
                ListId = faker.Random.Guid().ToString()
            };

            _listRepository.Setup(x => x.GetByIdAsync(query.ListId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductsList)null);

            // Act

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert

            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
