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
    public class GetListByNameQueryTests
    {
        private readonly Mock<IListRepository> _repository;

        private readonly GetListByNameQueryHandler _handler;

        public GetListByNameQueryTests()
        {
            _repository = new Mock<IListRepository>();

            _handler = new GetListByNameQueryHandler(
                _repository.Object
            );
        }

        [Fact]
        public async Task GetListByNameQuery_Success_ShouldReturnList()
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

            var query = new GetListByNameQuery
            {
                Name = list.Name
            };

            _repository.Setup(x => x.GetListByName(query.Name, It.IsAny<CancellationToken>()))
                .ReturnsAsync(list);

            // Act

            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert

            result.Should().BeEquivalentTo(list);
        }

        [Fact]
        public async Task GetListByNameQuery_Fail_WhenListNotExist()
        {
            var faker = new Faker();

            var query = new GetListByNameQuery
            {
                Name = faker.Random.String()
            };

            // Act

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert

            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
