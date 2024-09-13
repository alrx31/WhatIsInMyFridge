using Domain.Entities;

namespace Domain.Repository
{
    public interface IRecieptsRepository:IBaseRepository<Reciept>
    {
        Task<Reciept> GetRecieptByNameAsync(string name, CancellationToken cancellationToken);

        Task<List<Reciept>> GetAllRecieptsPaginationAsync(int page, int count, CancellationToken cancellationToken);
    }
}
