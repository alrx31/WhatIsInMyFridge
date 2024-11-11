using Application.UseCases.Queries;
using Application.UseCases.QueriesHandlers;
using Bogus;
using Domain.Entities;
using Domain.Repository;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.UseCases.Queries
{
    public class GetAllListsQueryTests
    {
        private readonly Mock<IListRepository> _listRepository;

        private readonly GetAllListsQueryHandler _handler;

        public GetAllListsQueryTests()
        {
            _listRepository = new Mock<IListRepository>();

            _handler = new GetAllListsQueryHandler(
                _listRepository.Object
            );
        }

        [Fact]
        public async Task GetAllListsQuery_Success_ShouldReturnLists()
        {
            var faker = new Faker();
            var lists = new List<ProductsList>
            {
                new ProductsList
                {
                    Id = faker.Random.Int().ToString(),
                    FridgeId = faker.Random.Int(),
                    BoxNumber = faker.Random.Int(),
                    Name = faker.Lorem.Word(),
                    Weight = faker.Random.Int(),
                    HowPackeges = faker.Random.Int(),
                    Price = faker.Random.Decimal(),
                    CreateData = faker.Date.Past()
                },
                new ProductsList
                {
                    Id = faker.Random.Int().ToString(),
                    FridgeId = faker.Random.Int(),
                    BoxNumber = faker.Random.Int(),
                    Name = faker.Lorem.Word(),
                    Weight = faker.Random.Int(),
                    HowPackeges = faker.Random.Int(),
                    Price = faker.Random.Decimal(),
                    CreateData = faker.Date.Past()
                }
            };

            _listRepository.Setup(x=>x.GetAllAsync(new CancellationToken())).ReturnsAsync(lists);

            var query = new GetAllListsQuery();

            // Act
            var result = await _handler.Handle(query, new CancellationToken());

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(lists);
        }
    }
}
