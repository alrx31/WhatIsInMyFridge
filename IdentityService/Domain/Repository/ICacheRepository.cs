using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface ICacheRepository
    {
        Task<T?> GetCacheData<T>(string key);

        Task SetCatcheData<T>(string key, T data, TimeSpan? expiry = null );
        
        Task RemoveCacheData(string key);
    }
}
