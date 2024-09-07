using Domain.Entities;

namespace Domain.Repository
{
    public interface IListRepository
    {
        Task AddList(ProductsList list);
        Task DeleteListById(string id);
        Task<ProductsList> GetListById(string id);
        Task<ProductsList> GetListByName(string name);
        Task UpdateList(ProductsList ls);
    }
}
