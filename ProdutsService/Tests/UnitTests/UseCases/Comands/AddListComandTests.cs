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
    public class AddListComandTests
    {
        private readonly Mock<IListRepository> _listRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly AddListComandHandler _handler;

        public AddListComandTests()
        {
            _listRepository = new Mock<IListRepository>();
            _mapper = new Mock<IMapper>();

            _handler = new AddListComandHandler(_listRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task AddListComand_Success_ShouldAddList()
        {
            var faker = new Faker();

            var comand = new AddListComand(new AddListDTO
            {
                Name = faker.Commerce.ProductName(),
                Weight = faker.Random.Int(1, 100),
                FridgeId = faker.Random.Int(1, 100),
                BoxNumber = faker.Random.Int(1, 100),
                Price = faker.Random.Decimal(1, 100)
            });

            // Act

            await _handler.Handle(comand, CancellationToken.None);

            // Assert

            _listRepository.Verify(x => x.AddAsync(It.IsAny<ProductsList>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddListComand_Fail_WhenListExist()
        {
            var faker = new Faker();
            var list = new ProductsList
            {
                Id = faker.Random.Int(1, 100).ToString(),
                Name = faker.Commerce.ProductName(),
                Weight = faker.Random.Int(1, 100),
                FridgeId = faker.Random.Int(1, 100),
                BoxNumber = faker.Random.Int(1, 100),
                Price = faker.Random.Decimal(1, 100)
            };

            var comand = new AddListComand(new AddListDTO { Name = list.Name });

            _listRepository.Setup(x => x.GetListByName(list.Name, It.IsAny<CancellationToken>())).ReturnsAsync(list);

            // Act

            Func<Task> act = async () => await _handler.Handle(comand, CancellationToken.None);

            // Assert

            await act.Should().ThrowAsync<AlreadyExistsException>();
        }
    }
}
