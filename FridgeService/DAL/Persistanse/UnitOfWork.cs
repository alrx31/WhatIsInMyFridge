using DAL.Entities;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Persistanse
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddFridge(Fridge fridge)
        {
            await _context.fridges.AddAsync(fridge);

            await this.CompleteAsync();
        }

        public async Task<Fridge?> GetFridge(int fridgeId)
        {
            return await _context.fridges.FindAsync(fridgeId);
        }

        public async Task RemoveFridge(int fridgeId)
        {
            var fridge = await _context.fridges.FindAsync(fridgeId);
            
            _context.productFridgeModels.RemoveRange(_context.productFridgeModels.Where(fm => fm.fridgeId == fridgeId));
            _context.userFridges.RemoveRange(_context.userFridges.Where(uf => uf.fridgeId == fridgeId));

            _context.fridges.Remove(fridge);

            await this.CompleteAsync();
        }

        public async Task AddUserToFridge(int fridgeId, int userId)
        {
            var fridge = await _context.fridges.FindAsync(fridgeId);

            var model = new UserFridge
            {
                fridgeId = fridgeId,
                fridge = fridge,
                userId = userId,
                LinkTime = DateTime.UtcNow
            };

            await _context.userFridges.AddAsync(model);

            await this.CompleteAsync();
        }

        public async Task RemoveUserFromFridge(int fridgeId, int userId)
        {
            var model = await _context.userFridges.FirstOrDefaultAsync(f => f.fridgeId == fridgeId && f.userId == userId);

            _context.userFridges.Remove(model);

            await this.CompleteAsync();
        }

        public async Task<List<int>> GetUsersFromFridge(int fridgeid)
        {
            return _context.userFridges.Where(m => m.fridgeId == fridgeid).Select(u => u.userId).ToList();
        }


        public async Task<Fridge> UpdateFridge(Fridge fridge)
        {
            _context.fridges.Update(fridge);

            await this.CompleteAsync();

            return fridge;
        }

        public async Task AddProductToFridge(ProductFridgeModel products)
        {
            await _context.productFridgeModels.AddAsync(products);

            await this.CompleteAsync();
        }

        public async Task AddProductsToFridge(List<ProductFridgeModel> products)
        {
            await _context.productFridgeModels.AddRangeAsync(products);

            await this.CompleteAsync();
        }

        public async Task RemoveProductFromFridge(int fridgeId, string productId)
        {
            var model = await _context.productFridgeModels.FirstOrDefaultAsync(f => f.fridgeId == fridgeId && f.productId == productId);

            _context.productFridgeModels.Remove(model);

            await this.CompleteAsync();
        }

        public async Task<List<ProductFridgeModel>> GetProductsFromFridge(int fridgeId)
        {
            return await _context.productFridgeModels.Where(m => m.fridgeId == fridgeId).ToListAsync();
        }

        public async Task<List<Fridge>> GetAllFridges()
        {
            return await _context.fridges.ToListAsync();
        }

    }
}
