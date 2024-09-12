using Domain.Entities;

namespace Domain.Repository
{
    public interface IListManageRepository
    {
        Task AddProductToList(ProductInList productInList);

        Task DeleteProductInList(string listId,string productId);

        Task<List<string>> GetListProducts(string listId);
    }
}
