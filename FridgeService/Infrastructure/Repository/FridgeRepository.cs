using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistanse;
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


    }
}
