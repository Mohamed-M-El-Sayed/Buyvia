using System.Collections;
using Microsoft.EntityFrameworkCore.Storage;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.BaseEntities;

namespace OnlineStore.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private Hashtable _repositories;
        private readonly OnlineStoreDbContext _dbContext;
        private IDbContextTransaction? _transaction;
        public ICateogoryRepository Categories { get; }
        public UnitOfWork(OnlineStoreDbContext dbContext)
        {
            _dbContext = dbContext;
            Categories = new CateogoryRepository(dbContext);
            _repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync(CancellationToken cancellationToken = default) => await _dbContext.SaveChangesAsync(cancellationToken);


        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var typeName = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(typeName))
            {
                // if not exits add to HashTable
                var respository = new GenericRepository<TEntity>(_dbContext);
                _repositories.Add(typeName, respository);
            }
            // typeof will return full name solution.namespace.typename
            // .name return type name only like product
            return _repositories[typeName] as IGenericRepository<TEntity>;
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        }
        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is null)
                throw new InvalidOperationException("No transaction started");

            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null; // avoid reuse 
        }
        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction is null) return;

            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}
