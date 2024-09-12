using Domain.Entities;
using Domain.Repository;
using MongoDB.Driver;

namespace Infrastructure.Persistance
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _products;

        public ProductRepository(ApplicationDbContext context)
        {
            _products = context.GetCollection<Product>("Products");
        }

        public async Task AddProduct(Product product)
        {
            await _products.InsertOneAsync(product);
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task DeleteProductById(string id)
        {
            await _products.DeleteOneAsync(p => p.Id == id);
        }

        public async Task UpdateProduct(Product product)
        {
            await _products.ReplaceOneAsync(p => p.Id == product.Id, product);
        }

        public async Task<Product> GetProductByName(string name)
        {
            return await _products.Find(p => p.Name == name).FirstOrDefaultAsync();
        }
        
        public async Task<List<Product>> GetAllProducts(int page, int count)
        {
            return await _products.Find(p => true).Skip((page - 1)* count).Limit(count).ToListAsync();
        }

        public async Task<List<Product>> GetProductRange(List<string> listProductsModels)
        {
            var listProducts = listProductsModels;
            var products = new List<Product>();

            foreach (var productId in listProducts)
            {
                var product = await GetProduct(productId);
                products.Add(product);
            }
            
            return products;
        }
    }
}
