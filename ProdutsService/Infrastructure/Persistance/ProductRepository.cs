using Domain.Entities;
using Domain.Repository;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
