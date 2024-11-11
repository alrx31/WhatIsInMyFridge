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
    public class GetListQueryTests
    {
        private readonly Mock<IListRepository> _repository;

        private readonly GetListQueryHandler _handler;

        public GetListQueryTests()
        {
            _repository = new Mock<IListRepository>();

            _handler = new GetListQueryHandler(
                _repository.Object
            );
        }

        [Fact]
        public async Task GetListQuery_Success_ShouldReturnList()
        {
            var faker = new Faker();
            var list = new ProductsList
            {
                Id = faker.Random.Guid().ToString(),
                FridgeId = faker.Random.Int(),
                BoxNumber = faker.Random.Int(),
                Name = faker.Random.String(),
                Weight = faker.Random.Int(),
                HowPackeges = faker.Random.Int(),
                Price = faker.Random.Decimal(),
                CreateData = faker.Date.Past()
            };

            _repository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(list);

            var request = new GetListQuery
            (
                list.Id
            );

            // Act

            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert

            result.Should().BeEquivalentTo(list);
        }

        [Fact]
        public async Task GetListQuery_Fail_WhenListNotExist()
        {
            var faker = new Faker();
            
            var query = new GetListQuery
            (
                faker.Random.Guid().ToString()
            );

            // Act

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert

            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
