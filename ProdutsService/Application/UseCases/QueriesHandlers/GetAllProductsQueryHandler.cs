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
            if (request.Count < 1)
            {
                throw new ValidationDataException("Count must be greater than 0");
            }

            if(request.Page < 1)
            {
                throw new ValidationDataException("Page must be greater than 0");
            }

            return await _productRepository.GetAllProducts(request.Page,request.Count);
        }
    }
}
