using System;
using System.Reflection;
using System.Runtime.Caching;

namespace AppMGL.DAL.Helper
{
    public static class ParameterCache
    {
        private static readonly ObjectCache _cache = MemoryCache.Default;
        private static CacheItemPolicy _policy;
        private static CacheEntryRemovedCallback _callback;

        public static void AddToCache(string cacheKeyName, PropertyInfo[] cacheItem, CacheItemPriority cacheItemPriority)
        {
            _callback = CachedItemRemovedCallback;
            _policy = new CacheItemPolicy
            {
                Priority = cacheItemPriority,
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(1000.00),
                RemovedCallback = _callback
            };
            _cache.Set(cacheKeyName, cacheItem, _policy);
        }

        public static PropertyInfo[] GetCachedItem(String cacheKeyName)
        {
            return (PropertyInfo[])_cache[cacheKeyName];
        }

        public static void RemoveCachedItem(String cacheKeyName)
        {
            if (_cache.Contains(cacheKeyName))
            {
                _cache.Remove(cacheKeyName);
            }
        }

        private static void CachedItemRemovedCallback(CacheEntryRemovedArguments arguments)
        {
            var strLog = String.Concat("Reason: ", arguments.RemovedReason.ToString(), 
                " | Key‐Name: ", arguments.CacheItem.Key, 
                " | Value‐Object: ", arguments.CacheItem.Value.ToString());
        }
    }
}