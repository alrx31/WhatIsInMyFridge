namespace DAL.Repositories
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}