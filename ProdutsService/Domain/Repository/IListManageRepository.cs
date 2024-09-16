using Domain.Entities;

namespace Domain.Repository
{
    public interface IListManageRepository:IBaseRepository<ProductInList>
    {
        Task DeleteProductInList(string listId, string productId, CancellationToken cancellationToken);
        
        Task<List<string>> GetListProducts(string listId, CancellationToken cancellationToken);

        Task<ProductInList> GetProductInLlist(string listId, string productId, CancellationToken cancellationToken);
    }
}
