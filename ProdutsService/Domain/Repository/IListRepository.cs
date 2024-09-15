using Domain.Entities;

namespace Domain.Repository
{
    public interface IListRepository:IBaseRepository<ProductsList>
    {
        Task<ProductsList> GetListByName(string name,CancellationToken cancellationToken);   
    }
}
