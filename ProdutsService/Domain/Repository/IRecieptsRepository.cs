using Domain.Entities;

namespace Domain.Repository
{
    public interface IRecieptsRepository
    {
        Task AddReciept(Reciept reciept);
        Task DeleteReciept(Reciept reciept);
        Task<List<Reciept>> GetAllReciepts(int page, int count);
        Task<Reciept> GetReciept(string recieptId);
        Task UpdateReciept(Reciept reciept);
    }
}
