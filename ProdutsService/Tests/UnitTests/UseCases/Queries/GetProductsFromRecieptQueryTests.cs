using Application.Exceptions;
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
    public class GetProductsFromRecieptQueryTests
    {
        private readonly Mock<IRecieptsRepository> _recieptRepository;
        private readonly Mock<IProductRepository> _productRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly GetProductsFromRecieptQueryHandler _handler;

        public GetProductsFromRecieptQueryTests()
        {
            _recieptRepository = new Mock<IRecieptsRepository>();
            _productRepository = new Mock<IProductRepository>();
            _mapper = new Mock<IMapper>();

            _handler = new GetProductsFromRecieptQueryHandler(
                _recieptRepository.Object,
                _productRepository.Object,
                _mapper.Object
            );
        }

        [Fact]
        public async Task GetProductsFromRecieptQuery_Success_ShouldReturnProducts()
        {
            var faker = new Faker();
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
                },
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

            var reciept = new Reciept
            {
                Id = faker.Random.Guid().ToString(),
                Products = products,
                CookDuration = new TimeSpan(),
                Portions = faker.Random.Int(),
                Kkal = faker.Random.Int(),
                Name = faker.Random.String()
            };

            _recieptRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(reciept);
            
            var query = new GetProductsFromRecieptQuery
            (
                reciept.Id
            );

            // Act

            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert

            result.Should().BeEquivalentTo(products);
        }

        [Fact]
        public async Task GetProductsFromRecieptQuery_Fail_WhenRecieptNotExist()
        {
            var faker = new Faker();

            var query = new GetProductsFromRecieptQuery
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
