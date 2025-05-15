using ExamApp.Application.Contracts.Caching;
using ExamApp.Caching;

namespace ExamApp.API.Extensions
{
    public static class CachingExtensions
    {
        public static IServiceCollection AddCustomCaching(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<ICacheService, CacheService>();
            return services;
        }
    }
}
