using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infrastructure.Persistance
{
    public class ApplicationDbContext
    {
        private readonly IMongoDatabase _database;

        public ApplicationDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<TEntity> GetCollection<TEntity>(string name)
    {
        return _database.GetCollection<TEntity>(name);
    }
    }
}
