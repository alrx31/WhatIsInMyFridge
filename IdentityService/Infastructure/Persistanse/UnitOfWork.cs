using Domain.Entities;
using Domain.Repository;
using Infastructure.Persistanse;
using System;
using System.Threading.Tasks;

namespace Identity.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _contex;

        public ICacheRepository CacheRepository { get; set; }

        public IUserRepository UserRepository { get; set; }

        public UnitOfWork(
            ApplicationDbContext contex,
            IUserRepository userRepository,
            ICacheRepository cacheRepository
            )
        {
            _contex = contex;
            UserRepository = userRepository;
            CacheRepository = cacheRepository;
        }

        public async Task CompleteAsync()
        {
            await _contex.SaveChangesAsync();
        }
    }
}
