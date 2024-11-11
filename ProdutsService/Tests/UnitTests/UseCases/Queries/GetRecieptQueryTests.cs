using Application.Exceptions;
using Application.UseCases.Queries;
using Application.UseCases.QueriesHandlers;
using AutoMapper;
using Bogus;
using Domain.Entities;
using Domain.Repository;
using FluentAssertions;
using Infrastructure.Persistanse.Protos;
using Moq;

namespace Tests.UnitTests.UseCases.Queries
{
    public class GetRecieptQueryTests
    {
        private readonly Mock<IRecieptsRepository> _recieptsRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly GetRecieptQueryHandler _handler;

        public GetRecieptQueryTests()
        {
            _recieptsRepository = new Mock<IRecieptsRepository>();
            _mapper = new Mock<IMapper>();

            _handler = new GetRecieptQueryHandler(
                _recieptsRepository.Object,
                _mapper.Object
            );
        }

        [Fact]
        public async Task GetRecieptQuery_Success_ShouldReturnReciept()
        {
            var faker = new Faker();
            var reciept = new Reciept
            {
                Id = faker.Random.Guid().ToString(),
                Products = [],
                CookDuration = new TimeSpan(),
                Portions = faker.Random.Int(),
                Kkal = faker.Random.Int(),
                Name = faker.Random.String()
            };

            _recieptsRepository.Setup(x => x.GetByIdAsync(reciept.Id, CancellationToken.None))
                .ReturnsAsync(reciept);

            var request = new GetRecieptQuery(reciept.Id);

            // Act

            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert

            result.Should().BeEquivalentTo(reciept);
        }

        [Fact]
        public async Task GetRecieptQuery_Fail_WhenRecieptNotEixst()
        {
            var faker = new Faker();

            var query = new GetRecieptQuery(faker.Random.Guid().ToString());

            // Act

            Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

            // Assert

            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
