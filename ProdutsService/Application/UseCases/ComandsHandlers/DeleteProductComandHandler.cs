using Application.Exceptions;
using Application.UseCases.Comands;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.ComandsHandlers
{
    public class DeleteProductComandHandler:IRequestHandler<DeleteProductComand>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductComandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(DeleteProductComand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id,cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("Product not found");
            }

            await _productRepository.DeleteAsync(request.Id,cancellationToken);
        }
    }
}
