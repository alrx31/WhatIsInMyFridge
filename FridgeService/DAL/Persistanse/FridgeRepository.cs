﻿using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL.Persistanse
{
    public class FridgeRepository
        (
            ApplicationDbContext context,
            ILogger<FridgeRepository> logger
        ) : IFridgeRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<FridgeRepository> _logger = logger;

        public async Task AddFridge(Fridge fridge)
        {
            await _context.fridges.AddAsync(fridge);
        }

        public async Task<Fridge?> GetFridge(int fridgeId)
        {
            return await _context.fridges.FindAsync(fridgeId);
        }

        public async Task<List<Fridge>> GetFridgeByUserId(int userId)
        {
            return await _context.userFridges
                .Where(uf => uf.userId == userId)
                .Join(_context.fridges, uf => uf.fridgeId, f => f.id, (uf, f) => f)
                .ToListAsync();
        }

        public async Task RemoveFridge(int fridgeId)
        {
            var fridge = await _context.fridges.FindAsync(fridgeId);

            _context.productFridgeModels.RemoveRange(_context.productFridgeModels.Where(fm => fm.fridgeId == fridgeId));

            _context.userFridges.RemoveRange(_context.userFridges.Where(uf => uf.fridgeId == fridgeId));

            _context.fridges.Remove(fridge);
        }

        public async Task AddUserToFridge(UserFridge model)
        {
            await _context.userFridges.AddAsync(model);
        }

        public async Task RemoveUserFromFridge(int fridgeId, int userId)
        {
            var model = await _context.userFridges.FirstOrDefaultAsync(f => f.fridgeId == fridgeId && f.userId == userId);

            _context.userFridges.Remove(model);
        }

        public async Task<List<int>> GetUsersFromFridge(int fridgeid)
        {
            return _context.userFridges.Where(m => m.fridgeId == fridgeid).Select(u => u.userId).ToList();
        }


        public async Task<Fridge> UpdateFridge(Fridge fridge)
        {
            _context.fridges.Update(fridge);

            return fridge;
        }

        public async Task AddProductToFridge(ProductFridgeModel products)
        {
            await _context.productFridgeModels.AddAsync(products);
        }

        public async Task AddProductsToFridge(List<ProductFridgeModel> products)
        {
            await _context.productFridgeModels.AddRangeAsync(products);
        }

        public async Task RemoveProductFromFridge(int fridgeId, string productId)
        {
            var model = await _context.productFridgeModels.FirstOrDefaultAsync(f => f.fridgeId == fridgeId && f.productId == productId);

            _context.productFridgeModels.Remove(model);
        }

        public async Task<ProductFridgeModel> GetProductFromFridge(int fridgeId, string productId)
        {
            return await _context.productFridgeModels.FirstOrDefaultAsync(m => m.productId == productId && m.fridgeId == fridgeId);
        }

        public async Task<List<ProductFridgeModel>> GetProductsFromFridge(int fridgeId)
        {
            return await _context.productFridgeModels.Where(m => m.fridgeId == fridgeId).ToListAsync();
        }

        public async Task<List<Fridge>> GetAllFridges()
        {
            return await _context.fridges.ToListAsync();
        }

        public async Task UpdateProductInFridge(ProductFridgeModel model)
        {
            _context.productFridgeModels.Update(model);
        }

        public async Task<Fridge> GetFridgeBySerialAndBoxNumber(string serial, int boxNumber)
        {
            return await _context.fridges.FirstOrDefaultAsync(f => f.serial == serial && f.boxNumber == boxNumber);
        }
    }
}
