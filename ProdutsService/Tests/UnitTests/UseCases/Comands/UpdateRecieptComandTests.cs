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
    public class UpdateRecieptComandTests
    {
        private readonly Mock<IRecieptsRepository> _recieptsRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly UpdateRecieptComandHandler _handler;

        public UpdateRecieptComandTests()
        {
            _recieptsRepository = new Mock<IRecieptsRepository>();
            _mapper = new Mock<IMapper>();

            _handler = new UpdateRecieptComandHandler(_recieptsRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task UpdateRecieptComand_Success_ShouldUpdateReciept()
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

            var comand = new UpdateRecieptComand(
                reciept.Id,
                new AddRecieptDTO
                {
                    Name = faker.Commerce.ProductName(),
                    CookDuration = new TimeSpan(),
                    Portions = faker.Random.Int(1, 100),
                    Kkal = faker.Random.Int(1, 100)
                }
            );

            _recieptsRepository.Setup(x => x.GetByIdAsync(reciept.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(reciept);

            // Act

            await _handler.Handle(comand, CancellationToken.None);

            // Assert

            _recieptsRepository.Verify(x => x.UpdateAsync(It.IsAny<Reciept>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateRecieptComand_Fail_WhenRecieptNotFound()
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

            var comand = new UpdateRecieptComand(
                reciept.Id,
                new AddRecieptDTO
                {
                    Name = faker.Commerce.ProductName(),
                    CookDuration = new TimeSpan(),
                    Portions = faker.Random.Int(1, 100),
                    Kkal = faker.Random.Int(1, 100)
                }
            );

            // Act

            Func<Task> act = async () => await _handler.Handle(comand, CancellationToken.None);

            // Assert

            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
