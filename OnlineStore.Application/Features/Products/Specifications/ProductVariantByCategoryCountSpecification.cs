using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Products;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Products.Specifications
{
    public class ProductVariantByCategoryCountSpecification : BaseSpecification<ProductVariant>
    {
        public ProductVariantByCategoryCountSpecification(
            IEnumerable<int> categoryIds,
            decimal? minPrice,
            decimal? maxPrice,
            string? searchTerm)
        {
            Criteria = v =>
                v.IsActive &&
                v.IsDefault &&
                v.Product.Status == ProductStatus.Published &&
                v.Product.CategoryId.HasValue &&
                categoryIds.Contains(v.Product.CategoryId.Value) &&
                (!minPrice.HasValue || v.Price >= minPrice) &&
                (!maxPrice.HasValue || v.Price <= maxPrice) &&
                (string.IsNullOrWhiteSpace(searchTerm) ||
                 v.Product.Name.Contains(searchTerm) ||
                 v.Product.ShortDescription.Contains(searchTerm) ||
                 v.Product.Brand.Name.Contains(searchTerm));

            AsNoTracking();
        }
    }
}
