using Domain.Entities;
using Domain.Repositories;
using Infastructure.Middlewares.Exceptions;
using Infrastructure.Persistanse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
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
            
            if(fridge == null)
            {
                throw new NotFoundException("fridge not found");
            }

            _context.productFridgeModels.RemoveRange(_context.productFridgeModels.Where(fm=>fm.fridgeId == fridgeId));
            
            _context.userFridges.RemoveRange(_context.userFridges.Where(uf=>uf.fridgeId == fridgeId));
            
            _context.fridges.Remove(fridge);
        
        }

        public async Task AddUserToFridge(int fridgeId, int userId)
        {
            var fridge = await _context.fridges.FindAsync(fridgeId);
            
            if(fridge == null)
            {
                throw new NotFoundException("fridge not found");
            }
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
            
            if(model == null)
            {
                throw new NotFoundException("user not found in fridge");
            }

            _context.userFridges.Remove(model);

        }

        public async Task<List<User>> GetUsersFromFridge(int fridgeid)
        {
            // TODO: use grpc to get users
            throw new NotImplementedException("need to use gRPC");
        }

        public async Task<Fridge> UpdateFridge(Fridge fridge,int fridgeId)
        {
            var model = await _context.fridges.FindAsync(fridgeId);
            
            model.model = fridge.model;
            model.boxNumber = fridge.boxNumber;
            model.name = fridge.name;
            model.products = fridge.products;

            return model;
        }
    }
}
