using Application.Exceptions;
using Application.UseCases.Queries;
using Domain.Entities;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.QueriesHandlers
{
    public class GetAllProductsQueryHandler
        (
            IProductRepository productRepository
        ):IRequestHandler<GetAllProductsQuery,List<Product>>
    {
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<List<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var prs =  await _productRepository.GetAllPaginationAsync(request.Page,request.Count,cancellationToken);
            return prs;
        }
    }
}
