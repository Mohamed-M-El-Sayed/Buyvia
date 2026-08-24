using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductVariants.Specifications
{
    public class LowStockVariantsSpecification : BaseSpecification<ProductVariant>
    {
        public LowStockVariantsSpecification(IEnumerable<int> variantIds)
        {
            // Prevent sending low-stock alerts more than once every 24 hours
            var cutoff = DateTime.UtcNow.AddHours(-24);

            Criteria = v => variantIds.Contains(v.Id)
                          && v.Stock <= v.StockThreshold
                          && (v.LowStockAlertedAt == null || v.LowStockAlertedAt < cutoff);
        }
    }
}
