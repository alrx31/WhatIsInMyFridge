﻿using Domain.Repository;
using Grpc.Core;
using Infrastructure.Persistanse.Protos;

namespace Infrastructure.Persistance
{
    public class ProductgRPCService
        (
            IProductRepository productRepository
        ):Products.ProductsBase
    {
        private readonly IProductRepository _productRepository = productRepository;

        public override async Task<ProductssResponse> GetProducts(ProductsIds ids, ServerCallContext context)
        {
            var cancellationToken = context.CancellationToken;

            var products = await _productRepository.GetProductRange(ids.Ids.ToList<string>(),cancellationToken);

            var response = new ProductssResponse();

            for(int i = 0; i < products.Count; i++)
            {
                var model = new Product
                {
                    Id = products[i].Id,
                    Name = products[i].Name,
                    PricePerKilo = (float) products[i].PricePerKilo,
                    ExpTime = products[i].ExpirationTime.ToString()
                };

                response.Products.Add(model);
            }

            return response;
        }
    }
}
