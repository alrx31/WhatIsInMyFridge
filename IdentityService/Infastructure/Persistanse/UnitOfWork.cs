using Domain.Entities;
using Domain.Repository;
using Infastructure.Persistanse;
using System;
using System.Threading.Tasks;

namespace Identity.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _contex;
        private readonly IUserRepository _userRepository;
        private readonly ICacheRepository _cacheRepository;

        public UnitOfWork(
            ApplicationDbContext contex,
            IUserRepository userRepository,
            ICacheRepository cacheRepository
            )
        {
            _contex = contex;
            _userRepository = userRepository;
            _cacheRepository = cacheRepository;
        }

        public async Task CompleteAsync()
        {
            await _contex.SaveChangesAsync();
        }

        // User Repository
        public async Task RegisterUser(User model)
        {
            await _userRepository.RegisterUser(model);
            
            await CompleteAsync();
        }

        public async Task<User?> GetUserByLogin(string email)
        {
            return await _userRepository.GetUserByLogin(email);
        }

        public async Task<RefreshTokenModel?> GetTokenModel(string email)
        {
            return await _userRepository.GetTokenModel(email);
        }

        public async Task UpdateRefreshTokenAsync(RefreshTokenModel identityUserTokenModel)
        {
            await _userRepository.UpdateRefreshTokenAsync(identityUserTokenModel);
            
            await CompleteAsync();
        }

        public async Task AddRefreshTokenField(RefreshTokenModel model)
        {
            await _userRepository.AddRefreshTokenField(model);
            
            await CompleteAsync();
        }

        public async Task CanselRefreshToken(int userId)
        {
            await _userRepository.CanselRefreshToken(userId);
            
            await CompleteAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _userRepository.GetUserById(id);
        }

        public async Task DeleteUser(int id)
        {
            await _userRepository.DeleteUser(id);
            
            await CompleteAsync();
        }

        public async Task DeleteRefreshTokenByUserId(int userId)
        {
            await _userRepository.DeleteRefreshTokenByUserId(userId);
            
            await CompleteAsync();
        }

        public async Task<User> UpdateUser(User model)
        {
            var user = await _userRepository.UpdateUser(model);
            
            await CompleteAsync();
            
            return user;
        }

        public async Task<List<User>> GetUsers(List<int> ids)
        {
            return await _userRepository.GetUsers(ids);
        }


        // Cache Repository
        public async Task<T?> GetCacheData<T>(string key)
        {
            return await _cacheRepository.GetCacheData<T>(key);
        }

        public async Task SetCatcheData<T>(string key, T data, TimeSpan? expiry = null)
        {
            await _cacheRepository.SetCatcheData<T>(key, data, expiry);
            
            await CompleteAsync();
        }

        public async Task RemoveCacheData(string key)
        {
            await _cacheRepository.RemoveCacheData(key);
            
            await CompleteAsync();
        }

        
    }
}
