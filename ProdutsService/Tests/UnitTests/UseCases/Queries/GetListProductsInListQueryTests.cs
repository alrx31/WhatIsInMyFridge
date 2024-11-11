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
    public class GetListProductsInListQueryTests
    {
        private readonly Mock<IListManageRepository> _listmanageRepository;
        private readonly Mock<IListRepository> _listRepository;
        private readonly Mock<IProductRepository> _productRepository;

        private readonly GetListProductsInListQueryHandler _handler;

        public GetListProductsInListQueryTests()
        {
            _listmanageRepository = new Mock<IListManageRepository>();
            _listRepository = new Mock<IListRepository>();
            _productRepository = new Mock<IProductRepository>();

            _handler = new GetListProductsInListQueryHandler(
                _listmanageRepository.Object,
                _listRepository.Object,
                _productRepository.Object
            );
        }

        [Fact]
        public async Task GetListProductsInListQuery_Success_ShouldReturnProducts()
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
            var products = new List<ProductInList> 
            {
                new ProductInList
                {
                    Id = faker.Random.Guid().ToString(),
                    ProductId = faker.Random.Guid().ToString(),
                    ListId = faker.Random.Guid().ToString(),
                    Count = faker.Random.Number(1, 10),
                    Cost = faker.Random.Decimal()
                }
            };

            var query = new GetListProductsInListQuery
            {
                ListId = list.Id
            };

            _listRepository.Setup(x => x.GetByIdAsync(query.ListId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(list);
            _listmanageRepository.Setup(x => x.GetProductsInLlist(query.ListId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            // Act

            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert

            result.Should().BeEquivalentTo(products);
        }

        [Fact]
        public async Task GetListProductsInListQuery_Fail_WhenListNotExist()
        {
            var faker = new Faker();

            var query = new GetListProductsInListQuery
            {
                ListId = faker.Random.Guid().ToString()
            };

            // Act

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert

            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
