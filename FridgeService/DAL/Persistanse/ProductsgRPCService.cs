using DAL.IRepositories;
using DAL.Persistanse.Protos;

namespace DAL.Persistanse
{
    public class ProductsgRPCService: IProductsgRPCService
    {
        private readonly Products.ProductsClient _productsClient;

        public ProductsgRPCService(Products.ProductsClient productsClient)
        {
            _productsClient = productsClient;
        }

        public async Task<List<DAL.Entities.Product>> GetProducts(List<string> ids)
        {
            var ProductsIds = new ProductsIds();
            ProductsIds.Ids.AddRange(ids);

            var reply = await _productsClient.GetProductsAsync(ProductsIds);
            
            return reply.Products.Select(p => new DAL.Entities.Product
            {
                Id = p.Id,
                Name = p.Name,
                PricePerKilo = (decimal)p.PricePerKilo,
                ExpirationTime = TimeSpan.Parse(p.ExpTime)
            }).ToList<DAL.Entities.Product>();
        }
    }
}
