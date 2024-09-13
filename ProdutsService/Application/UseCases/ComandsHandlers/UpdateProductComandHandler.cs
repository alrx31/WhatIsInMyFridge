using Application.Exceptions;
using Application.UseCases.Comands;
using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.ComandsHandlers
{
    public class UpdateProductComandHandler
        (
            IProductRepository productRepository,
            IMapper mapper
        ):IRequestHandler<UpdateProductComand>
    {
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(UpdateProductComand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id,cancellationToken);

            if(product is null)
            {
                throw new NotFoundException("Product not found");
            }

            _mapper.Map(request, product);

            await _productRepository.UpdateAsync(product,cancellationToken);
        }
    }
}
