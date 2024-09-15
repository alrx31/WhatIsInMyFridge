using Domain.Entities;

namespace Domain.Repository
{
    public interface IListManageRepository:IBaseRepository<ProductInList>
    {
        Task DeleteProductInList(string listId, string productId, CancellationToken cancellationToken);
        
        Task DevideProductInList(string id, string productId, int count, CancellationToken stoppingToken);
        
        Task<List<string>> GetListProducts(string listId, CancellationToken cancellationToken);
    }
}
