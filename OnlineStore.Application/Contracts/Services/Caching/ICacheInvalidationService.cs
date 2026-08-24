namespace OnlineStore.Application.Contracts.Services.Caching
{
    public interface ICacheInvalidationService
    {
        Task EvictByTagAsync(
               string tag,
               CancellationToken cancellationToken = default);
    }
}
