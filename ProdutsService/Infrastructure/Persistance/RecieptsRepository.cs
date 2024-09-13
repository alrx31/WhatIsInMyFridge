using Domain.Entities;
using Domain.Repository;
using MongoDB.Driver;

namespace Infrastructure.Persistance
{
    public class RecieptsRepository :BaseRepository<Reciept>, IRecieptsRepository
    {
        private readonly IMongoCollection<Reciept>_context;

        public RecieptsRepository(ApplicationDbContext context):base(context,"Reciepts")
        {
            _context = context.GetCollection<Reciept>("Reciepts");
        }

        public async Task<List<Reciept>> GetAllRecieptsPaginationAsync(int page, int count, CancellationToken cancellationToken)
        {
            return await _context.Find(Builders<Reciept>.Filter.Empty).Skip((page - 1) * count).Limit(count).ToListAsync(cancellationToken);
        }

        public Task<Reciept> GetRecieptByNameAsync(string name, CancellationToken cancellationToken)
        {
            return _context.Find(r => r.Name == name).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
