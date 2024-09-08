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
    public class ListManageRepository : IListManageRepository
    {
        private readonly IMongoCollection<ProductInList> _context;

        public ListManageRepository(ApplicationDbContext context)
        {
            _context = context.GetCollection<ProductInList>("ListProducts");
        }

        public async Task AddProductToList(ProductInList productInList)
        {
            await _context.InsertOneAsync(productInList);
        }

        public async Task DeleteProductInList(string listId, string productId)
        {
            await _context.DeleteOneAsync(p => p.ListId == listId && p.ProductId == productId);
        }

        public async Task<List<string>> GetListProducts(string listId)
        {
            return await _context.Find(p => p.ListId == listId).Project(p => p.ProductId).ToListAsync();
        }
    }
}
