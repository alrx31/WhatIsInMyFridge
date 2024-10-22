using DAL.Interfaces;
using DAL.Repositories;

namespace DAL.Persistanse
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IFridgeRepository FridgeRepository { get;}

        public UnitOfWork(
            ApplicationDbContext context,
            IFridgeRepository fridges
            )
        {
            _context = context;
            FridgeRepository = fridges;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
