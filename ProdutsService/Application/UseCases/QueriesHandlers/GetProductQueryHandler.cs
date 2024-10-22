using Application.UseCases.Queries;
using Domain.Entities;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.QueriesHandlers
{
    public class GetProductQueryHandler: IRequestHandler<GetProductQuery, Product>
    {
        private readonly IProductRepository _productRepository;

        public GetProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            return await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}
