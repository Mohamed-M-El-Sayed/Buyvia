using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Contracts.Specifications;
using OnlineStore.Domain.Entities.BaseEntities;
using OnlineStore.Infrastructure.Persistence.Specifications;

namespace OnlineStore.Infrastructure.Persistence.Repositories
{
    internal class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly OnlineStoreDbContext _dbContext;

        public GenericRepository(OnlineStoreDbContext dbContext)
        {

            _dbSet = dbContext.Set<TEntity>();
            _dbContext = dbContext;
        }

        private IQueryable<TEntity> ApplyTracking(IQueryable<TEntity> query, bool tracking)
            => tracking ? query : query.AsNoTracking();

        private IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includes)
        {

            foreach (var include in includes)
                query = query.Include(include);
            return query;
        }


        public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();
            query = ApplyTracking(query, tracking);
            query = ApplyIncludes(query, includes);
            return await query.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id, bool tracking = true, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();
            query = ApplyTracking(query, tracking);
            query = ApplyIncludes(query, includes);
            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
        }
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate, CancellationToken cancellationToken = default)
        {
            var query = _dbSet;
            if (predicate is not null)
                return await query.AnyAsync(predicate, cancellationToken);
            return await query.AnyAsync(cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddRangeAsync(entities, cancellationToken);
        }
        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.AddAsync(entity, cancellationToken);
        }

        public void Update(TEntity entity)
        {
            _dbContext.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbContext.Remove(entity);
        }
        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _dbContext.RemoveRange(entities);
        }


        public IQueryable<TEntity> Query(bool tracking = false)
        {
            return tracking
                ? _dbSet.AsQueryable()
                : _dbSet.AsNoTracking();
        }
        public async Task<int> GetCountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            return await ApplaySpecification(specification).CountAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            return await ApplaySpecification(specification).ToListAsync(cancellationToken);
        }
        public async Task<TEntity?> GetEntityWithSpecAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
        {
            return await ApplaySpecification(specification).FirstOrDefaultAsync(cancellationToken);
        }
        private IQueryable<TEntity> ApplaySpecification(ISpecification<TEntity> specification)
        {
            return SpecificationEvaluator<TEntity>.GenerateQuery(_dbSet, specification);
        }


    }
}