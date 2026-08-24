using System.Linq.Expressions;
using Hangfire;

namespace OnlineStore.Infrastructure.Services.BackgroundJobs;

public class HangfireBackgroundJobService : IBackgroundJobService
{
    public void AddOrUpdate<T>(
        string jobId,
        Expression<Func<T, Task>> job,
        string cronExpression)
    {
        RecurringJob.AddOrUpdate(
            jobId,
            job,
            cronExpression);
    }

    public void Enqueue<T>(
        Expression<Func<T, Task>> job)
    {
        BackgroundJob.Enqueue(job);
    }
}