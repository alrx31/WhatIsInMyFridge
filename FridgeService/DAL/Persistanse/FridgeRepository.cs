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
        ):IFridgeRepository
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

        public async Task RemoveFridge(int fridgeId)
        {
            var fridge = await _context.fridges.FindAsync(fridgeId);

            _context.productFridgeModels.RemoveRange(_context.productFridgeModels.Where(fm => fm.fridgeId == fridgeId));

            _context.userFridges.RemoveRange(_context.userFridges.Where(uf => uf.fridgeId == fridgeId));

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

        public async Task<List<ProductFridgeModel>> GetProductsFromFridge(int fridgeId)
        {
            return await _context.productFridgeModels.Where(m => m.fridgeId == fridgeId).ToListAsync();
        }

        public async Task<List<Fridge>> GetAllFridges()
        {
            return await _context.fridges.ToListAsync();
        }

        public async Task DevideProductFromFridge(int fridgeId, string productId, int count)
        {
            var model = await _context.productFridgeModels.FirstOrDefaultAsync(f => f.fridgeId == fridgeId && f.productId == productId);

            _logger.LogInformation("start devide");

            if (model is not null)
            {
                _logger.LogInformation("not null");
                model.count = model.count - count;
            }
            else
            {
                _logger.LogInformation("null");
            }

            _logger.LogInformation("calc");

            if(model.count < 0)
            {

                throw new Exception("invalid count of product");
            }

            if(model.count == 0)
            {
                _logger.LogInformation(" equals to 0");

                _context.productFridgeModels.Remove(model);
            }
            else
            {
                _logger.LogInformation("unequals to 0");

                _context.productFridgeModels.Update(model);
            }
            _logger.LogInformation("end devide");
        }
    }
}
