namespace OnlineStore.Application.Contracts.Services.BackgroundJobs
{
    public interface ICleanupPendingOrdersJob
    {
        Task ExecuteAsync();

    }
}
