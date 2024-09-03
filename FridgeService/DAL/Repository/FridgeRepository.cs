using DAL.Entities;
using DAL.Persistanse;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class FridgeRepository : IFridgeRepository
    {
        private readonly ApplicationDbContext _context;

        public FridgeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddFridge(Fridge fridge)
        {
            await _context.fridges.AddAsync(fridge);
        }

        public async Task<Fridge?> GetFridge(int fridgeId)
        {
            return await _context.fridges.FindAsync(fridgeId);
        }

        public async Task RemoveFridge(int fridgeId)
        {
            var fridge = await _context.fridges.FindAsync(fridgeId);

            _context.productFridgeModels.RemoveRange(_context.productFridgeModels.Where(fm=>fm.fridgeId == fridgeId));
            
            _context.userFridges.RemoveRange(_context.userFridges.Where(uf=>uf.fridgeId == fridgeId));
            
            _context.fridges.Remove(fridge);
        
        }

        public async Task AddUserToFridge(int fridgeId, int userId)
        {
            var fridge = await _context.fridges.FindAsync(fridgeId);
            
            // TODO: use grpc to check if user exists

            var model = new UserFridge
            {
                fridgeId = fridgeId,
                fridge = fridge,
                userId = userId,
                LinkTime = DateTime.UtcNow
            };

            await _context.userFridges.AddAsync(model);
        }

        public async Task RemoveUserFromFridge(int fridgeId, int userId)
        {
            var model = await _context.userFridges.FirstOrDefaultAsync(f=>f.fridgeId == fridgeId && f.userId == userId);

            _context.userFridges.Remove(model);
        }

        public async Task<List<User>> GetUsersFromFridge(int fridgeid)
        {
            // TODO: use grpc to get users
            throw new NotImplementedException("need to use gRPC");
        }

        public async Task<Fridge> UpdateFridge(Fridge fridge)
        {
            _context.Update(fridge);
            return fridge;
        }

        public async Task AddProductToFridge(List<ProductFridgeModel> products)
        {
            await _context.productFridgeModels.AddRangeAsync(products);
        }

        public async Task AddProductsToFridge(List<ProductFridgeModel> products)
        {
            await _context.productFridgeModels.AddRangeAsync(products);
        }

        public async Task RemoveProductFromFridge(int fridgeId, int productId)
        {
            var model = await _context.productFridgeModels.FirstOrDefaultAsync(f=>f.fridgeId == fridgeId && f.productId == productId);
          
            _context.productFridgeModels.Remove(model);
        }
    }
}
