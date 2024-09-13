using Application.Exceptions;
using Application.UseCases.Comands;
using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.ComandsHandlers
{
    public class AddProductToListComandHandler
        (
            IListManageRepository listManageRepository,
            IProductRepository productRepository,
            IListRepository listRepository,
            IMapper mapper
        ) :IRequestHandler<AddProductToListComand>
    {
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IListRepository _listRepository = listRepository;
        private readonly IListManageRepository _listManageRepository = listManageRepository;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(AddProductToListComand request, CancellationToken cancellationToken)
        {
            var list = await _listRepository.GetByIdAsync(request.ListId,cancellationToken);

            if (list is null)
            {
                throw new NotFoundException("List not found");
            }

            var product = await _productRepository.GetByIdAsync(request.Model.ProductId,cancellationToken);
        
            if(product is null)
            {
                throw new NotFoundException("Product not found");
            }

            await _listManageRepository.AddAsync(_mapper.Map<ProductInList>(request),cancellationToken);
        }

    }
}
