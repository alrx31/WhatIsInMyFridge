using Application.Exceptions;
using Application.UseCases.Comands;
using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.ComandsHandlers
{
    public class AddProductToRecieptComandHandler
        (
            IRecieptsRepository recieptsRepository,
            IProductRepository productRepository,
            IMapper mapper
        ) :IRequestHandler<AddProductToRecieptComand>
    {
        private readonly IRecieptsRepository _recieptsRepository = recieptsRepository;
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(AddProductToRecieptComand request, CancellationToken cancellationToken)
        {
            var reciept = await _recieptsRepository.GetByIdAsync(request.Model.RecieptId, cancellationToken);

            if (reciept is null)
            {
                throw new NotFoundException("Reciept not found");
            }

            var product = await _productRepository.GetByIdAsync(request.Model.ProductId, cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("Product not found");
            }

            var productInReciept = _mapper.Map<ProductInReciept>((product,request.Model.Weight));

            if(reciept.Products is null)
            {
                reciept.Products = new List<ProductInReciept>();
            }

            reciept.Products.Add(productInReciept);

            await _recieptsRepository.UpdateAsync(reciept, cancellationToken);
        }
    }
}
