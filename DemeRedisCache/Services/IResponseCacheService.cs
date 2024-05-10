using System;
using System.Threading.Tasks;

namespace DemeRedisCache.Services
{
    public interface IResponseCacheService
    {
        Task SetCacheReponseAsync(string cacheKey, object response, TimeSpan timeOut);
        Task<string> GetCacheResponseAsync(string cacheKey);
        Task RemoveCacheResponseAsync(string partern);
    }
}
