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
    public class DeleteRecieptComandTests
    {
        private readonly Mock<IRecieptsRepository> _recieptsRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly DeleteRecieptComandHandler _handler;

        public DeleteRecieptComandTests()
        {
            _recieptsRepository = new Mock<IRecieptsRepository>();
            _mapper = new Mock<IMapper>();

            _handler = new DeleteRecieptComandHandler(_recieptsRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task DeleteRecieptComand_Success_ShouldDeleteReciept()
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
            
            var comand = new DeleteRecieptComand(reciept.Id);

            _recieptsRepository.Setup(x => x.GetByIdAsync(reciept.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(reciept);

            // Act

            await _handler.Handle(comand, CancellationToken.None);

            // Assert

            _recieptsRepository.Verify(x => x.DeleteAsync(reciept.Id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteRecieptComand_Fail_WhenRecieptNotExist()
        {
            var faker = new Faker();

            var comand = new DeleteRecieptComand(faker.Random.Int(1, 100).ToString());

            // Act

            Func<Task> act = async () => await _handler.Handle(comand, CancellationToken.None);

            // Assert

            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
