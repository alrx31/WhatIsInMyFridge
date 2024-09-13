using Domain.Entities;

namespace Domain.Repository
{
    public interface IUnitOfWork
    {
        ICacheRepository CacheRepository { get; }
        IUserRepository UserRepository { get; }
        Task CompleteAsync();
    }
}