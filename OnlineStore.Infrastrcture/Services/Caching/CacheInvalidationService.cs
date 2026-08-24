using Microsoft.AspNetCore.OutputCaching;
using OnlineStore.Application.Contracts.Services.Caching;

namespace OnlineStore.Infrastructure.Services.Caching
{
    public class CacheInvalidationService(IOutputCacheStore outputCacheStore) : ICacheInvalidationService
    {
        public async Task EvictByTagAsync(string tag, CancellationToken cancellationToken = default)
         => await outputCacheStore.EvictByTagAsync(tag, cancellationToken);

    }
}
