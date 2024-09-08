﻿using Application.UseCases.Queries;
using Domain.Entities;
using Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
            var list = await _listRepository.GetListById(request.ListId);

            if (list is null)
            {
                throw new Exception("List not found");
            }

            var listProductsModels = await _listManageRepository.GetListProducts(request.ListId);

            return await _productRepository.GetProductRange(listProductsModels);
        }
    }
}
