using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface ICacheRepository
    {
        Task<T?> GetCacheData<T>(string key);

        Task SetCatcheData<T>(string key, T data, TimeSpan? expiry = null);

        Task RemoveCacheData(string key);
    }
}