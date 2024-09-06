using Domain.Entities;

namespace Domain.Repository
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();

        // User Repository

        Task RegisterUser(User model);

        Task<User?> GetUserByLogin(string email);

        Task<RefreshTokenModel?> GetTokenModel(string email);

        Task UpdateRefreshTokenAsync(RefreshTokenModel identityUserTokenModel);

        Task AddRefreshTokenField(RefreshTokenModel model);

        Task CanselRefreshToken(int userId);

        Task<User?> GetUserById(int id);

        Task DeleteUser(int id);

        Task DeleteRefreshTokenByUserId(int userId);

        Task<User> UpdateUser(User model);

        Task<List<User>> GetUsers(List<int> ids);

        // Cache Repository

        Task<T?> GetCacheData<T>(string key);

        Task SetCatcheData<T>(string key, T data, TimeSpan? expiry = null);

        Task RemoveCacheData(string key);
    }
}