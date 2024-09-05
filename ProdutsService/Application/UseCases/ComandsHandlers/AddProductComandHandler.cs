using Application.UseCases.Comands;
using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.ComandsHandlers
{
    public class AddProductComandHandler(
            IProductRepository repository,
            IMapper mapper
        ) :IRequestHandler<AddProductComand>
    {
        private readonly IProductRepository _repository = repository; 
        private readonly IMapper _mapper = mapper;

        public async Task Handle(AddProductComand request, CancellationToken cancellationToken)
        {
            await _repository.AddProduct(_mapper.Map<Product>(request));
        }
    }
}
