using ExamApp.Application.Contracts.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace ExamApp.Caching
{
    public class CacheService(IMemoryCache memoryCache) : ICacheService
    {
        public Task<T?> GetAsync<T>(string cacheKey)
        {
            if (memoryCache.TryGetValue(cacheKey, out T? value))
            {
                return Task.FromResult(value);
            }
            return Task.FromResult(default(T));
        }

        public Task AddAsync<T>(string cacheKey, T value, TimeSpan exprTimeSpan)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = exprTimeSpan
            };
            memoryCache.Set(cacheKey, value, cacheEntryOptions);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string cacheKey)
        {
            memoryCache.Remove(cacheKey);
            return Task.CompletedTask;
        }
        
        public Task<bool> ExistsAsync(string cacheKey)
        {
            return Task.FromResult(memoryCache.TryGetValue(cacheKey, out _));
        }
    }
}
