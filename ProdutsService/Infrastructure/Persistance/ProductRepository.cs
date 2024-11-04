using Domain.Entities;
using Domain.Repository;
using MongoDB.Driver;

namespace Infrastructure.Persistance
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly IMongoCollection<Product> _products;

        public ProductRepository(ApplicationDbContext context):base(context, "Products")
        {
            _products = context.GetCollection<Product>("Products");
        }

        public async Task<Product> GetProductByName(string name,CancellationToken cancellationToken)
        {
            return await _products.Find(p => p.Name == name).FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetProductRange(List<string> listProductsModels,CancellationToken cancellationToken)
        {
            var listProducts = listProductsModels;
            var products = new List<Product>();

            foreach (var productId in listProducts)
            {
                var product = await this.GetByIdAsync(productId,cancellationToken);
                products.Add(product);
            }
            
            return products;
        }

        public async Task<List<Product>> GetAllPaginationAsync(int page, int count,CancellationToken cancellationToken)
        {
            var prs = await _products.Find(Builders<Product>.Filter.Empty).Skip((page - 1) * count).Limit(count).ToListAsync(cancellationToken);
            prs.Add(new Product());
            return prs;
        }
    }
}
