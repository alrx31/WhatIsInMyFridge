using Domain.Entities;

namespace Domain.Repository
{
    public interface IProductRepository
    {
        Task AddProduct(Product product);

        Task<Product> GetProduct(string id);

        Task DeleteProductById(string id);

        Task UpdateProduct(Product product);
        
        Task<Product> GetProductByName(string name);

        Task<List<Product>> GetAllProducts(int page, int count);
        
        Task<List<Product>> GetProductRange(List<string> listProductsModels);
    }
}
