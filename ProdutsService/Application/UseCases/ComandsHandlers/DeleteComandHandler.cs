using Application.UseCases.Comands;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.ComandsHandlers
{
    public class DeleteComandHandler:IRequestHandler<DeleteProductComand>
    {
        private readonly IProductRepository _productRepository;

        public DeleteComandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task Handle(DeleteProductComand request, CancellationToken cancellationToken)
        {
            await _productRepository.DeleteProductById(request.Id);
        }
    }
}
