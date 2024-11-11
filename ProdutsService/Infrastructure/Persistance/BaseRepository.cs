using Domain.Repository;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Persistance
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly IMongoCollection<TEntity> _collection;

        public BaseRepository(ApplicationDbContext database, string collectionName)
        {
            _collection = database.GetCollection<TEntity>(collectionName);
        }

        public async Task<TEntity> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse(id));

            return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _collection.Find(Builders<TEntity>.Filter.Empty).ToListAsync(cancellationToken);
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await _collection.InsertOneAsync(entity, null, cancellationToken);
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse((entity as dynamic).Id));

            await _collection.ReplaceOneAsync(filter, entity, new ReplaceOptions(), cancellationToken);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse(id));

            await _collection.DeleteOneAsync(filter, cancellationToken);
        }
    }

}
