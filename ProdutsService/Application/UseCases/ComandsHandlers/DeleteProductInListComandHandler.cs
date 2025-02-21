﻿using Application.Exceptions;
using Application.UseCases.Comands;
using AutoMapper;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.ComandsHandlers
{
    public class DeleteProductInListComandHandler
        (
            IListManageRepository listManageRepository,
            IListRepository listRepository,
            IProductRepository productRepository,
            IMapper mapper
        ):IRequestHandler<DeleteProductInListComand>
    {

        private readonly IListRepository _listRepository = listRepository;
        private readonly IListManageRepository _listManageRepository = listManageRepository;
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(DeleteProductInListComand request, CancellationToken cancellationToken)
        {
            var list = await _listRepository.GetByIdAsync(request.ListId,cancellationToken);

            if (list is null)
            {
                throw new NotFoundException("List not found");
            }

            var product = await _productRepository.GetByIdAsync(request.ProductId,cancellationToken);

            if (product is null)
            {
                throw new NotFoundException("Product not found");
            }

            await _listManageRepository.DeleteProductInList(request.ListId,request.ProductId,cancellationToken);
        }
    
    }
}
