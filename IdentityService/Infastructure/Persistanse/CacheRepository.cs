using Domain.Repository;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Infastructure.Persistanse
{
    public class CacheRepository : ICacheRepository
    {

        private readonly IDistributedCache _redis;

        public CacheRepository(IDistributedCache redis)
        {
            _redis = redis;
        }


        public async Task<T?> GetCacheData<T>(string key)
        {
            var data = await _redis.GetStringAsync(key);

            if (data is null)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(data);
        }

        public async Task RemoveCacheData(string key)
        {
            await _redis.RemoveAsync(key);
        }

        public async Task SetCatcheData<T>(string key, T data, TimeSpan? expiry = null)
        {
            await _redis.SetStringAsync(key, JsonSerializer.Serialize(data), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
            });
        }
    }
}
