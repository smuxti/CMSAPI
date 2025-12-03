using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Core.Interfaces
{
    public interface IRedisCacheService
    {
        Task SetCacheValueAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task<T> GetCacheValueAsync<T>(string key);
        Task<bool> DeleteCacheValueAsync(string key);
        Task<dynamic> GetCacheValueAsynca(string key);
    }
}
