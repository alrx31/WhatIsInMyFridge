using Domain.Repository;
using Infastructure.Persistanse;
using System;
using System.Threading.Tasks;

namespace Identity.Infrastructure
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
