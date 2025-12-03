using Merchants.Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Merchants.Infrastructure.Repositories
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _redis;
        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }
        public async Task<bool> DeleteCacheValueAsync(string key)
        {
            var db = _redis.GetDatabase();
            var json = await db.StringGetAsync(key);
            await db.KeyDeleteAsync(key);
            return true;
        }
        public async Task<T> GetCacheValueAsync<T>(string key)
        {
            var db = _redis.GetDatabase();
            var json = await db.StringGetAsync(key);
            return json.HasValue ? JsonSerializer.Deserialize<T>(json) : default;
        }
        public async Task<dynamic> GetCacheValueAsynca(string key)
        {
            var db = _redis.GetDatabase();
            var json = await db.StringGetAsync(key);
            return json;
        }


        public async Task SetCacheValueAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var db = _redis.GetDatabase();
            if (await db.KeyExistsAsync(key)) { await db.KeyDeleteAsync(key); }
            var json = JsonSerializer.Serialize(value);
            await db.StringSetAsync(key, json, expiration);
        }
    }
}
