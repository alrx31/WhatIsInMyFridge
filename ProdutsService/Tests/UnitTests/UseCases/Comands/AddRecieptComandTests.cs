using Application.DTO;
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
    public class AddRecieptComandTests
    {
        private readonly Mock<IRecieptsRepository> _recieptsRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly AddRecieptComandHandler _handler;

        public AddRecieptComandTests()
        {
            _recieptsRepository = new Mock<IRecieptsRepository>();
            _mapper = new Mock<IMapper>();

            _handler = new AddRecieptComandHandler(_recieptsRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task AddRecieptComand_Success_ShouldAddReciept()
        {
            var faker = new Faker();

            var comand = new AddRecieptComand(new AddRecieptDTO
            {
                Name = faker.Commerce.ProductName(),
                CookDuration = faker.Date.Timespan(),
                Portions = faker.Random.Int(1, 100),
                Kkal = faker.Random.Int(1, 100)
            });

            // Act

            await _handler.Handle(comand, CancellationToken.None);

            // Assert

            _recieptsRepository.Verify(x => x.AddAsync(It.IsAny<Reciept>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddRecieptComand_Fail_WhenRecieptExist()
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

            var comand = new AddRecieptComand
            (
                new AddRecieptDTO
                {
                    Name = reciept.Name
                }
            );

            _recieptsRepository.Setup(x=>x.GetRecieptByNameAsync(comand.Model.Name,It.IsAny<CancellationToken>()))
                .ReturnsAsync(reciept);

            // Act

            Func<Task> act = async () => await _handler.Handle(comand, CancellationToken.None);

            // Assert

            await act.Should().ThrowAsync<AlreadyExistsException>();
        }
    }
}
