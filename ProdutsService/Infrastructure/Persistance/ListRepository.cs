using Domain.Entities;
using Domain.Repository;
using MongoDB.Driver;

namespace Infrastructure.Persistance
{
    public class ListRepository : BaseRepository<ProductsList> ,IListRepository
    {
        private readonly IMongoCollection<ProductsList> _lists;

        public ListRepository(ApplicationDbContext context):base(context, "Lists")
        {
            _lists = context.GetCollection<ProductsList>("Lists");
        }

        public async Task<ProductsList> GetListByName(string name,CancellationToken cancellationToken)
        {
            return await _lists.Find(l => l.Name == name).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ProductsList> GetListbyFridgeId(int fridgeId, CancellationToken cancellationToken)
        {
            return await _lists.Find(l => l.FridgeId == fridgeId).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
