namespace OnlineStore.Application.Contracts.Services.BackgroundJobs
{
    public interface ILowStockCheckJob
    {
        Task ExecuteAsync(IEnumerable<int> productVariantIds);
    }
}
