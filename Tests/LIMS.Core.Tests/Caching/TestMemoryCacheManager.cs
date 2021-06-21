using LIMS.Core.Caching;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace LIMS.Core.Tests.Caching
{
    public class TestMemoryCacheManager : MemoryCacheManager
    {
        public override Task SetAsync(string key, object data, int cacheTime)
        {
            return Task.CompletedTask;
        }
        public override async Task<T> GetAsync<T>(string key, Func<Task<T>> acquire)
        {
            return await acquire();
        }
        public TestMemoryCacheManager(IMemoryCache cache, IMediator mediator) : base(cache, mediator)
        {
        }
    }
}
