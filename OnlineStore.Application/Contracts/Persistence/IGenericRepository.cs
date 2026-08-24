using System.Linq.Expressions;
using OnlineStore.Application.Contracts.Specifications;
using OnlineStore.Domain.Entities.BaseEntities;

namespace OnlineStore.Application.Contracts.Persistence
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = true, params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
        Task<TEntity?> GetEntityWithSpecAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
        Task<int> GetCountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

        Task<TEntity?> GetByIdAsync(int id, bool tracking = true, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        public IQueryable<TEntity> Query(bool tracking = false);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate, CancellationToken cancellation = default);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
    }
}
