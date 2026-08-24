using Microsoft.EntityFrameworkCore;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Infrastructure.Persistence.Repositories
{
    internal class CateogoryRepository : GenericRepository<ProductCategory>, ICateogoryRepository
    {
        private readonly OnlineStoreDbContext _dbContext;
        public CateogoryRepository(OnlineStoreDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProductCategory?> GetCategoryTreeByIdAsync(int id, bool tracking = true, CancellationToken cancellationToken = default)
        {
            var category = await _dbContext.Categories
                .Include(c => c.SubCategories)
                .ThenInclude(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            return category;
        }

        public async Task<IEnumerable<int>> GetLeafCategoryIdsAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            var categories = await _dbContext.Categories.ToListAsync(cancellationToken);

            var result = new List<int>();
            void GetChildren(int id)
            {
                var children = categories.Where(c => c.ParentId == id).ToList();
                if (!children.Any())
                {
                    result.Add(id); // leaf category
                    return;
                }

                foreach (var child in children)
                {
                    GetChildren(child.Id);
                }

            }
            GetChildren(categoryId);
            return result;
        }


        public async Task<int> GetDepthAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            var depth = 1;

            var category = await _dbContext.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    c => c.Id == categoryId,
                    cancellationToken);

            if (category is null)
                throw new NotFoundException(
                    nameof(ProductCategory),
                    categoryId.ToString());

            while (category.ParentId.HasValue)
            {
                depth++;

                category = await _dbContext.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        c => c.Id == category.ParentId.Value,
                        cancellationToken);

                if (category is null)
                    break;
            }

            return depth;
        }
    }
}
