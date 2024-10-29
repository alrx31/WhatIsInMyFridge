using Application.DTO;
using Application.Exceptions;
using Application.UseCases.Comands;
using Application.UseCases.ComandsHandlers;
using AutoMapper;
using Bogus;
using Docker.DotNet.Models;
using Domain.Entities;
using Domain.Repository;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.UseCases.Comands
{
    public class AddProductToRecieptComandTests
    {
        private readonly Mock<IRecieptsRepository> _recieptsRepository;
        private readonly Mock<IProductRepository> _productRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly AddProductToRecieptComandHandler _handler;

        public AddProductToRecieptComandTests()
        {
            _recieptsRepository = new Mock<IRecieptsRepository>();
            _productRepository = new Mock<IProductRepository>();
            _mapper = new Mock<IMapper>();

            _handler = new AddProductToRecieptComandHandler(_recieptsRepository.Object, _productRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task AddProductToRecieptComand_Success_ShouldAddProductToReciept()
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

            var comand = new AddProductToRecieptComand(new AddProductToRecieptDTO
            {
                RecieptId = reciept.Id,
                ProductId = product.Id,
                Weight = faker.Random.Int(1, 100)
            });

            _recieptsRepository.Setup(x => x.GetByIdAsync(reciept.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(reciept);
            _productRepository.Setup(x => x.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act

            await _handler.Handle(comand, CancellationToken.None);

            // Assert

            _recieptsRepository.Verify(x => x.UpdateAsync(It.IsAny<Reciept>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddProductToRecieptComand_Fail_WhenRecieptNotFound()
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

            var comand = new AddProductToRecieptComand(new AddProductToRecieptDTO
            {
                RecieptId = reciept.Id,
                ProductId = product.Id,
                Weight = faker.Random.Int(1, 100)
            });

            _recieptsRepository.Setup(x => x.GetByIdAsync(reciept.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Reciept)null);
            _productRepository.Setup(x => x.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act

            Func<Task> act = async () => await _handler.Handle(comand, CancellationToken.None);

            // Assert

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Reciept not found");
        }

        [Fact]
        public async Task AddProductToRecieptComand_Fail_WhenProductNotExist()
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

            var comand = new AddProductToRecieptComand(new AddProductToRecieptDTO
            {
                RecieptId = reciept.Id,
                ProductId = faker.Random.Int(1, 100).ToString(),
                Weight = faker.Random.Int(1, 100)
            });

            _recieptsRepository.Setup(x => x.GetByIdAsync(reciept.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(reciept);

            // Act

            Func<Task> act = async () => await _handler.Handle(comand, CancellationToken.None);

            // Assert

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Product not found");
        }
    }
}
