﻿using Domain.Repositories;

namespace Infrastructure.Persistanse
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _contex;
        
        public UnitOfWork(ApplicationDbContext contex)
        {
            _contex = contex;
        }

        public async Task CompleteAsync()
        {
            await _contex.SaveChangesAsync();
        }
        
    }
}
