using Domain.Entities;
using Domain.Repository;

namespace Infrastructure.Persistance
{
    public class RecieptsRepository : IRecieptsRepository
    {
        public Task AddReciept(Reciept reciept)
        {
            throw new NotImplementedException();
        }

        public Task DeleteReciept(Reciept reciept)
        {
            throw new NotImplementedException();
        }

        public Task<List<Reciept>> GetAllReciepts(int page, int count)
        {
            throw new NotImplementedException();
        }

        public Task<Reciept> GetReciept(string recieptId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateReciept(Reciept reciept)
        {
            throw new NotImplementedException();
        }
    }
}
