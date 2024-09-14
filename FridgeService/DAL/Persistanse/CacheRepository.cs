using DAL.Interfaces;
using DAL.IRepositories;
using StackExchange.Redis;
using System.Text.Json;

namespace DAL.Persistanse
{
    public class CacheRepository: ICacheRepository
    {
        private readonly IDatabase _redis;

        public CacheRepository(IConnectionMultiplexer redis)
        {
            _redis = redis.GetDatabase();
        }

        public async Task<T?> GetCacheData<T>(string key)
        {
            var data = await _redis.StringGetAsync(key);

            if (data.IsNull)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(data);
        }

        public async Task RemoveCacheData(string key)
        {
            await _redis.KeyDeleteAsync(key);
        }

        public async Task SetCatcheData<T>(string key, T data, TimeSpan? expiry = null)
        {
            await _redis.StringSetAsync(key, JsonSerializer.Serialize(data), expiry);
        }
    }
}
