namespace Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}