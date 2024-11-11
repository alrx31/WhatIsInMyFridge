using Application.Exceptions;
using Application.UseCases.Comands;
using AutoMapper;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.ComandsHandlers
{
    public class DeleteProductFromRecieptComandHandler
        (
            IRecieptsRepository recieptsRepository,
            IProductRepository productRepository,
            IMapper mapper
        ) : IRequestHandler<DeleteProductFromRecieptComand>
    {
        private readonly IRecieptsRepository _recieptsRepository = recieptsRepository;
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(DeleteProductFromRecieptComand request, CancellationToken cancellationToken)
        {
            var reciept = await _recieptsRepository.GetByIdAsync(request.RecieptId, cancellationToken);

            if (reciept is null)
            {
                throw new NotFoundException("Reciept not found");
            }

            var product = await _productRepository.GetByIdAsync(request.ProductId,cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("Product not found");
            }


            reciept?.Products?.RemoveAll(p => p.Id == request.ProductId);

            await _recieptsRepository.UpdateAsync(reciept, cancellationToken);
        }
    }
}
