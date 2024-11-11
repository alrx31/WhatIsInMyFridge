using Application.DTO;
using Application.Exceptions;
using Application.UseCases.Comands;
using Application.UseCases.ComandsHandlers;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Bogus;
using Domain.Entities;
using Domain.Repository;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.UseCases.Comands
{
    public class DeleteProductFromRecieptComandTests
    {
        private readonly Mock<IRecieptsRepository> _recieptsRepository;
        private readonly Mock<IProductRepository> _productRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly DeleteProductFromRecieptComandHandler _handler;

        public DeleteProductFromRecieptComandTests()
        {
            _recieptsRepository = new Mock<IRecieptsRepository>();
            _productRepository = new Mock<IProductRepository>();
            _mapper = new Mock<IMapper>();

            _handler = new DeleteProductFromRecieptComandHandler(_recieptsRepository.Object, _productRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task DeleteProductFromRecieptComand_Success_ShouldDeleteProductFromReciept()
        {
            var faker = new Faker();

            var reciept = new Reciept
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
                CookDuration = new TimeSpan(),
                Portions = faker.Random.Int(1, 100),
                Kkal = faker.Random.Int(1, 100),
                Products = []
            };

            var product = new Product
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
                PricePerKilo = faker.Random.Decimal(1, 100),
                ExpirationTime = new TimeSpan(),
            };

            var comand = new DeleteProductFromRecieptComand(
                reciept.Id,
                product.Id
            );

            _recieptsRepository.Setup(x => x.GetByIdAsync(reciept.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(reciept);

            _productRepository.Setup(x => x.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act

            await _handler.Handle(comand, new CancellationToken());

            // Assert

            reciept.Products.Should().NotContain(p => p.Id == product.Id);
        }

        [Fact]
        public async Task DeleteProductFromRecieptComand_Fail_WhenRecieptNotExist()
        {
            var faker = new Faker();

            var reciept = new Reciept
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
                CookDuration = new TimeSpan(),
                Portions = faker.Random.Int(1, 100),
                Kkal = faker.Random.Int(1, 100),
                Products = []
            };

            var product = new Product
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
                PricePerKilo = faker.Random.Decimal(1, 100),
                ExpirationTime = new TimeSpan(),
            };

            var comand = new DeleteProductFromRecieptComand(
                reciept.Id,
                product.Id
            );

            _recieptsRepository.Setup(x => x.GetByIdAsync(reciept.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Reciept)null);

            _productRepository.Setup(x => x.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act

            Func<Task> act = async () => await _handler.Handle(comand, new CancellationToken());

            // Assert

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Reciept not found");
        }

        [Fact]
        public async Task DeleteProductFromRecieptComand_Fail_WhenProductNotExist()
        {
            var faker = new Faker();

            var reciept = new Reciept
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
                CookDuration = new TimeSpan(),
                Portions = faker.Random.Int(1, 100),
                Kkal = faker.Random.Int(1, 100),
                Products = []
            };

            var comand = new DeleteProductFromRecieptComand(
                reciept.Id,
                faker.Random.Int(1, 100).ToString()
            );

            _recieptsRepository.Setup(x => x.GetByIdAsync(reciept.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(reciept);

            // Act

            Func<Task> act = async () => await _handler.Handle(comand, new CancellationToken());

            // Assert

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Product not found");
        }
    }
}
