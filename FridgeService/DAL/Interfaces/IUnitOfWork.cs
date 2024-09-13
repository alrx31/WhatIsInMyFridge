using DAL.Interfaces;

namespace DAL.Repositories
{
    public interface IUnitOfWork
    {
        IFridgeRepository FridgeRepository { get; }
        Task CompleteAsync();
    }
}