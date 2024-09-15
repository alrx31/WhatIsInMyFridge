using Domain.Entities;
using Domain.Repository;
using MongoDB.Driver;

namespace Infrastructure.Persistance
{
    public class ListManageRepository :BaseRepository<ProductInList>, IListManageRepository
    {
        private readonly IMongoCollection<ProductInList> _context;

        public ListManageRepository(ApplicationDbContext context):base(context, "ListProducts")
        {
            _context = context.GetCollection<ProductInList>("ListProducts");
        }

        public Task DeleteProductInList(string listId, string productId, CancellationToken cancellationToken)
        {
            var filter = Builders<ProductInList>.Filter.Eq("ListId", listId) & Builders<ProductInList>.Filter.Eq("ProductId", productId);

            return _context.DeleteOneAsync(filter, cancellationToken);
        }

        public async Task DevideProductInList(string id, string productId, int count, CancellationToken stoppingToken)
        {
            var filter = Builders<ProductInList>.Filter.Eq("ListId", id) &
                         Builders<ProductInList>.Filter.Eq("ProductId", productId);

            var product = await _context.Find(filter).FirstOrDefaultAsync(stoppingToken);

            if (product == null)
            {
                throw new InvalidOperationException("Product not found in the list.");
            }

            if(count == 0)
            {
                await _context.DeleteOneAsync(filter, stoppingToken);
            }

            if (product.Count - count < 0)
            {
                throw new InvalidOperationException("Product count cannot be negative.");
            }

            if (product.Count - count == 0)
            {
                await _context.DeleteOneAsync(filter, stoppingToken);
            }
            else
            {
                var update = Builders<ProductInList>.Update.Set(p => p.Count, product.Count - count);

                await _context.UpdateOneAsync(filter, update, cancellationToken: stoppingToken);
            }
        }

        public Task<List<string>> GetListProducts(string listId, CancellationToken cancellationToken)
        {
            var filter = Builders<ProductInList>.Filter.Eq("ListId", listId);

            return _collection.Find(filter).Project(p => p.ProductId).ToListAsync(cancellationToken);
        }
    }
}
