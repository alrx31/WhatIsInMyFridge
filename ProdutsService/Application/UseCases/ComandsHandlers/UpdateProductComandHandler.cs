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
            var product = await _productRepository.GetProduct(request.Id);

            if(product is null)
            {
                throw new NotFoundException("Product not found");
            }

            var pr = _mapper.Map<Product>(request);
            pr.Id = product.Id;

            await _productRepository.UpdateProduct(pr);
        }
    }
}
