﻿using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemeRedisCache.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public ResponseCacheService(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer)
        {
            _distributedCache = distributedCache;
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<string> GetCacheResponseAsync(string cacheKey)
        {
            var cacheReponse = await _distributedCache.GetStringAsync(cacheKey);
            return string.IsNullOrEmpty(cacheReponse) ? null : cacheReponse;
        }

        public async Task SetCacheReponseAsync(string cacheKey, object response, TimeSpan timeOut)
        {
            if (response == null)
                return;

            var serializerResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            await _distributedCache.SetStringAsync(cacheKey, serializerResponse, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeOut
            });
        }

        public async Task RemoveCacheResponseAsync(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
                throw new ArgumentException("Value cannot be null or whitespace");

            await foreach (var key in GetKeyAsync(pattern + "*"))
            {
                await _distributedCache.RemoveAsync(key);
            }
        }

        private async IAsyncEnumerable<string> GetKeyAsync(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
                throw new ArgumentException("Value cannot be null or whitespace");

            foreach (var endPoint in _connectionMultiplexer.GetEndPoints())
            {
                var server = _connectionMultiplexer.GetServer(endPoint);
                foreach (var key in server.Keys(pattern: pattern))
                {
                    yield return key.ToString(); // yield return và tiếp tục vòng lặp
                }
            }
        }
    }
}
