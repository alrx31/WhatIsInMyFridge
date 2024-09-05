using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
