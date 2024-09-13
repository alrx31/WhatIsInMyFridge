using Application.Exceptions;
using Application.UseCases.Queries;
using Domain.Entities;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.QueriesHandlers
{
    public class GetListProductsQueryHandler
        (
            IListManageRepository listManageRepository,
            IListRepository listRepository,
            IProductRepository productRepository
        ):IRequestHandler<GetListProductsQuery,List<Product>>
    {
        private readonly IListManageRepository _listManageRepository = listManageRepository;
        private readonly IListRepository _listRepository = listRepository;
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<List<Product>> Handle(GetListProductsQuery request, CancellationToken cancellationToken)
        {
            var list = await _listRepository.GetByIdAsync(request.ListId,cancellationToken);

            if (list is null)
            {
                throw new NotFoundException("List not found");
            }

            var listProductsModels = await _listManageRepository.GetListProducts(request.ListId,cancellationToken);

            return await _productRepository.GetProductRange(listProductsModels,cancellationToken);
        }
    }
}
