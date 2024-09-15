﻿using Domain.Entities;
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

        public Task<List<string>> GetListProducts(string listId, CancellationToken cancellationToken)
        {
            var filter = Builders<ProductInList>.Filter.Eq("ListId", listId);

            return _collection.Find(filter).Project(p => p.ProductId).ToListAsync(cancellationToken);
        }
    }
}
