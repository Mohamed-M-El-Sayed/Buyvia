using System.Reflection;
using MediatR;
using OnlineStore.Application.Contracts.Services.Caching;

namespace OnlineStore.Application.Common.Behaviors
{
    public class CacheInvalidationBehavior<TRequest, TResponse>
        (ICacheInvalidationService cacheInvalidationService) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // execute the handler first
            var response = await next();

            // get cache invalidation attributes from command
            var attribute = request.GetType()
                .GetCustomAttributes<InvalidateCacheAttribute>();
            foreach (var attr in attribute)
            {
                await cacheInvalidationService.EvictByTagAsync(attr.Tag, cancellationToken);
            }
            return response;
        }
    }
}
