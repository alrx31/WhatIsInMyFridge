using DAL.Entities;

namespace DAL.IRepositories
{
    public interface IgRPCService
    {
        Task<List<User>> GetUsers(List<int> ids);

        Task<bool> CheckUserExist(int userId);
    }
}
