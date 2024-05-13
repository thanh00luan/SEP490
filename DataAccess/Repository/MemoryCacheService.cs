using DataAccess.IRepository;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class MemoryCacheService : ICache
    {
        private MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public void Set(string key, object value, TimeSpan expiration)
        {
            _cache.Set(key, value, expiration);
        }

        public T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
