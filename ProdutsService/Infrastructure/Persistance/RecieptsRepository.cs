using Domain.Entities;
using Domain.Repository;
using MongoDB.Driver;

namespace Infrastructure.Persistance
{
    public class RecieptsRepository : IRecieptsRepository
    {
        private readonly IMongoCollection<Reciept>_context;

        public RecieptsRepository(ApplicationDbContext context)
        {
            _context = context.GetCollection<Reciept>("Reciepts");
        }

        public async Task AddReciept(Reciept reciept)
        {
            await _context.InsertOneAsync(reciept);
        }

        public async Task DeleteReciept(Reciept reciept)
        {
            await _context.DeleteOneAsync(r => r.Id == reciept.Id);
        }

        public async Task<List<Reciept>> GetAllReciepts(int page, int count)
        {
            return await _context.Find(r => true).Skip(page * count).Limit(count).ToListAsync();
        }

        public async Task<Reciept> GetReciept(string recieptId)
        {
            return await _context.Find(r => r.Id == recieptId).FirstOrDefaultAsync();
        }

        public async Task UpdateReciept(Reciept reciept)
        {
            await _context.ReplaceOneAsync(r => r.Id == reciept.Id, reciept);
        }
    }
}
