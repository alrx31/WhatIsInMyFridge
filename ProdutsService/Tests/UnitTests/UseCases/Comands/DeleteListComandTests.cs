using Application.Exceptions;
using Application.UseCases.Comands;
using Application.UseCases.ComandsHandlers;
using Bogus;
using Domain.Entities;
using Domain.Repository;
using FluentAssertions;
using Moq;

namespace Tests.UnitTests.UseCases.Comands
{
    public class DeleteListComandTests
    {
        private readonly Mock<IListRepository> _listRepository;

        private readonly DeleteListComandHandler _handler;

        public DeleteListComandTests()
        {
            _listRepository = new Mock<IListRepository>();

            _handler = new DeleteListComandHandler(_listRepository.Object);
        }

        [Fact]
        public async Task DeleteListComand_Success_ShouldDeleteList()
        {
            var faker = new Faker();

            var list = new ProductsList
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
            };

            _listRepository.Setup(x => x.GetByIdAsync(list.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(list);

            // Act

            await _handler.Handle(new DeleteListComand(list.Id), CancellationToken.None);

            // Assert

            _listRepository.Verify(x => x.DeleteAsync(list.Id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteListComand_Fail_WhenListNotExist()
        {
            var faker = new Faker();

            var list = new ProductsList
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
            };
            
            // Act

            Func<Task> act = async () => await _handler.Handle(new DeleteListComand(list.Id), CancellationToken.None);

            // Assert

            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
