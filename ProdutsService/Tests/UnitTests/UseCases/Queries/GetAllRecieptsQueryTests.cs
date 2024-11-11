using Application.UseCases.Queries;
using Application.UseCases.QueriesHandlers;
using AutoMapper;
using Bogus;
using Domain.Entities;
using Domain.Repository;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.UseCases.Queries
{
    public class GetAllRecieptsQueryTests
    {
        private readonly Mock<IRecieptsRepository> _recieptsRepository;
        private readonly Mock<IMapper> _mapper;
        
        private readonly GetAllRecieptsQueryHandler _handler;

        public GetAllRecieptsQueryTests()
        {
            _recieptsRepository = new Mock<IRecieptsRepository>();
            _mapper = new Mock<IMapper>();

            _handler = new GetAllRecieptsQueryHandler(
                _recieptsRepository.Object,
                _mapper.Object
            );
        }

        [Fact]
        public async Task GetAllRecieptsQuery_Success_ShouldReturnReciepts()
        {
            var faker = new Faker();
            var reciepts = new List<Reciept>
            {
                new Reciept
                {
                    Id = faker.Random.Guid().ToString(),
                    Name = faker.Random.String(),
                    CookDuration = TimeSpan.FromMinutes(faker.Random.Number(1, 60)),
                    Portions = faker.Random.Number(1, 10),
                    Kkal = faker.Random.Number(100, 1000),
                    Products = []
                },
                new Reciept
                {
                    Id = faker.Random.Guid().ToString(),
                    Name = faker.Random.String(),
                    CookDuration = TimeSpan.FromMinutes(faker.Random.Number(1, 60)),
                    Portions = faker.Random.Number(1, 10),
                    Kkal = faker.Random.Number(100, 1000),
                    Products = []
                }
            };

            var query = new GetAllRecieptsQuery
            {
                Page = faker.Random.Number(1, 10),
                Count = faker.Random.Number(1, 10)
            };

            _recieptsRepository.Setup(x => x.GetAllRecieptsPaginationAsync(query.Page, query.Count, It.IsAny<CancellationToken>()))
                .ReturnsAsync(reciepts);

            // Act

            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            
            result.Should().BeEquivalentTo(reciepts);
        }
    }
}
