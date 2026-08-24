using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Contracts.Persistence
{
    public interface ICateogoryRepository : IGenericRepository<ProductCategory>
    {
        Task<ProductCategory?> GetCategoryTreeByIdAsync(int id, bool tracking = false, CancellationToken cancellationToken = default);
        Task<IEnumerable<int>> GetLeafCategoryIdsAsync(int categoryId, CancellationToken cancellationToken = default);
        Task<int> GetDepthAsync(int categoryId, CancellationToken cancellationToken = default);
    }
}
