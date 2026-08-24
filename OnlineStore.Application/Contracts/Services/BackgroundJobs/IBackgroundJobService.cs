using System.Linq.Expressions;

public interface IBackgroundJobService
{
    void AddOrUpdate<T>(
        string jobId,
        Expression<Func<T, Task>> job,
        string cronExpression);

    void Enqueue<T>(
        Expression<Func<T, Task>> job);
}