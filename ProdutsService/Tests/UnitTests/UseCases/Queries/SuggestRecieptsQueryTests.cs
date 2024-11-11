using Application.UseCases.QueriesHandlers;
using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using Microsoft.Extensions.Logging;
using Bogus;
using Moq;
using FluentAssertions;
using Application.Exceptions;

namespace Tests.UnitTests.UseCases.Queries
{
    public class SuggestRecieptsQueryTests
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IRecieptsRepository> _recieptsRepository;
        private readonly Mock<ILogger<SuggestRecieptsQueryHandler>> _logger;

        private readonly SuggestRecieptsQueryHandler _handler;

        public SuggestRecieptsQueryTests()
        {
            _mapper = new Mock<IMapper>();
            _recieptsRepository = new Mock<IRecieptsRepository>();
            _logger = new Mock<ILogger<SuggestRecieptsQueryHandler>>();

            _handler = new SuggestRecieptsQueryHandler(
                _mapper.Object,
                _recieptsRepository.Object,
                _logger.Object
            );
        }

        [Fact]
        public async Task SuggestRecieptsQuery_Success_ShouldGiveSuggest()
        {
            var faker = new Faker();

            var reciepts = new List<Reciept>
            {
                new Reciept
                {
                    Id = faker.Random.Guid().ToString(),
                    Products = [],
                    CookDuration = new TimeSpan(),
                    Portions = faker.Random.Int(),
                    Kkal = faker.Random.Int(),
                    Name = faker.Random.String()
                }
            };

            var products = new List<ProductInReciept>
            {
                new ProductInReciept
                {
                    Id = faker.Random.Guid().ToString(),
                    Name = faker.Random.String(),
                    PricePerKilo = faker.Random.Decimal(),
                    ExpirationTime = new TimeSpan(),
                    Lists = new List<ProductInList>(),
                    Weight = faker.Random.Int()
                }
            };

            var request = new SuggestRecieptsQuery
            {
                products = products
            };

            _recieptsRepository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(reciepts);

            // Act

            var result = await _handler.Handle(request, new CancellationToken());

            // Assert

            result.Should().BeEquivalentTo(reciepts[0]);
        }

        [Fact]
        public async Task SuggestRecieptsQuery_Fail_WhenListIsEmpty()
        {
            var request = new SuggestRecieptsQuery
            {
                products = new List<ProductInReciept>()
            };

            // Act

            Func<Task> act = async () => await _handler.Handle(request, new CancellationToken());

            // Assert

            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task SuggestRecieptsQuery_Fail_WhenNoReciepts()
        {
            var faker = new Faker();

            var request = new SuggestRecieptsQuery
            {
                products = new List<ProductInReciept>
                {
                    new ProductInReciept
                    {
                        Id = faker.Random.Guid().ToString(),
                        Name = faker.Random.String(),
                        PricePerKilo = faker.Random.Decimal(),
                        ExpirationTime = new TimeSpan(),
                        Lists = new List<ProductInList>(),
                        Weight = faker.Random.Int()
                    }
                }
            };

            _recieptsRepository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Reciept>());

            // Act

            Func<Task> act = async () => await _handler.Handle(request, new CancellationToken());

            // Assert

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("No reciepts available.");
        }

        [Fact]
        public async Task SuggestRecieptsQuery_Fail_WhenNoSuitableReciepts()
        {
            var faker = new Faker();

            var reciepts = new List<Reciept>
            {
                new Reciept
                {
                    Id = faker.Random.Guid().ToString(),
                    Products = new List<ProductInReciept>
                    {
                        new ProductInReciept
                        {
                            Id = faker.Random.Guid().ToString(),
                            Name = faker.Random.String(),
                            PricePerKilo = faker.Random.Decimal(),
                            ExpirationTime = new TimeSpan(),
                            Lists = new List<ProductInList>(),
                            Weight = faker.Random.Int()
                        }
                    },
                    CookDuration = new TimeSpan(),
                    Portions = faker.Random.Int(),
                    Kkal = faker.Random.Int(),
                    Name = faker.Random.String()
                }
            };

            var request = new SuggestRecieptsQuery
            {
                products = new List<ProductInReciept>
                {
                    new ProductInReciept
                    {
                        Id = faker.Random.Guid().ToString(),
                        Name = faker.Random.String(),
                        PricePerKilo = faker.Random.Decimal(),
                        ExpirationTime = new TimeSpan(),
                        Lists = new List<ProductInList>(),
                        Weight = faker.Random.Int()
                    }
                }
            };

            _recieptsRepository.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(reciepts);

            // Act

            Func<Task> act = async () => await _handler.Handle(request, new CancellationToken());

            // Assert

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("No suitable reciepts found.");
        }
    }
}
