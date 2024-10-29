using Application.Exceptions;
using Application.UseCases.Comands;
using Application.UseCases.ComandsHandlers;
using AutoMapper;
using Bogus;
using Domain.Entities;
using Domain.Repository;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.UseCases.Comands
{
    public class DeleteProductInListComandTests
    {
        private readonly Mock<IListManageRepository> _listManageRepository;
        private readonly Mock<IListRepository> _listRepository;
        private readonly Mock<IProductRepository> _productRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly DeleteProductInListComandHandler _handler;

        public DeleteProductInListComandTests()
        {
            _listManageRepository = new Mock<IListManageRepository>();
            _listRepository = new Mock<IListRepository>();
            _productRepository = new Mock<IProductRepository>();
            _mapper = new Mock<IMapper>();

            _handler = new DeleteProductInListComandHandler(_listManageRepository.Object, _listRepository.Object, _productRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task DeleteProductInListComand_Success_ShouldDeleteProductInList()
        {
            var faker = new Faker();
            var list = new ProductsList
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
            };
            var product = new Product
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
            };
            var comand = new DeleteProductInListComand(
                list.Id,
                product.Id
            );

            _listRepository.Setup(x => x.GetByIdAsync(list.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(list);

            _productRepository.Setup(x => x.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act

            await _handler.Handle(comand, CancellationToken.None);

            // Assert

            _listManageRepository.Verify(x => x.DeleteProductInList(list.Id, product.Id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteProductInListComand_Fail_WhenListNotFound()
        {
            var faker = new Faker();
            var list = new ProductsList
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
            };
            var product = new Product
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
            };
            var comand = new DeleteProductInListComand(
                list.Id,
                product.Id
            );

            _productRepository.Setup(x => x.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            // Act

            Func<Task> act = async () => await _handler.Handle(comand, CancellationToken.None);

            // Assert

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("List not found");
        }

        [Fact]
        public async Task DeleteProductInListComand_Fail_WhenProductNotFound()
        {
            var faker = new Faker();
            var list = new ProductsList
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
            };
            var product = new Product
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
            };
            var comand = new DeleteProductInListComand(
                list.Id,
                product.Id
            );

            _listRepository.Setup(x => x.GetByIdAsync(list.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(list);

            // Act

            Func<Task> act = async () => await _handler.Handle(comand, CancellationToken.None);

            // Assert

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Product not found");
        }
    }
}
