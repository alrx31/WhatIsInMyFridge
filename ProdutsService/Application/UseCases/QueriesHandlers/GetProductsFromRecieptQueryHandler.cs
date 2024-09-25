using Application.Exceptions;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.QueriesHandlers
{
    public class GetProductsFromRecieptQueryHandler
        (
            IRecieptsRepository recieptsRepository,
            IProductRepository productRepository,
            IMapper mapper
        ) :IRequestHandler<GetProductsFromRecieptQuery,List<Product>>
    {
        private readonly IRecieptsRepository _recieptsRepository = recieptsRepository;
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<List<Product>> Handle(GetProductsFromRecieptQuery request, CancellationToken cancellationToken)
        {
            var reciept = await _recieptsRepository.GetByIdAsync(request.RecieptId, cancellationToken);

            if (reciept is null)
            {
                throw new NotFoundException("Reciept not found");
            }

            return reciept.Products;
        }
    }
}
