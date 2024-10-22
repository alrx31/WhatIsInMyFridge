namespace Domain.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(string id, CancellationToken cancellationToken);
        
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        
        Task AddAsync(TEntity entity, CancellationToken cancellationToken);
        
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
        
        Task DeleteAsync(string id, CancellationToken cancellationToken);
    }
}
