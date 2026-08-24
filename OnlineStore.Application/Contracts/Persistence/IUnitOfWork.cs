using OnlineStore.Domain.Entities.BaseEntities;

namespace OnlineStore.Application.Contracts.Persistence
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync(CancellationToken cancellationToken = default);
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        ICateogoryRepository Categories { get; }

        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    }
}
