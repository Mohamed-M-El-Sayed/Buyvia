using OnlineStore.Application.Common.Models;
using OnlineStore.Application.Common.Specifications;
using OnlineStore.Application.Features.Products.Queries.GetProductsByCategory;
using OnlineStore.Domain.Entities.Products;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Products.Specifications
{
    public class ProductVariantByCategorySpecification
        : BaseSpecification<ProductVariant>
    {
        public ProductVariantByCategorySpecification(
            IEnumerable<int> categoryIds,
            int pageNumber,
            int pageSize,
            decimal? minPrice,
            decimal? maxPrice,
            ProductSortField? sortBy,
            SortDirection? sortDirection,
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

            ApplyInclude(v => v.Product);
            ApplyInclude(v => v.Product.Brand);
            ApplyInclude(v => v.Product.Category);
            ApplyInclude(v => v.Discount!);
            ApplyInclude("Options.Value");
            ApplyInclude(v => v.Images.Where(i => i.IsMainImage));

            if (sortBy == ProductSortField.Price)
            {
                if (sortDirection == SortDirection.Descending)
                    ApplyOrderByDesc(v => v.Price);
                else
                    ApplyOrderBy(v => v.Price);
            }
            else if (sortBy == ProductSortField.Name)
            {
                if (sortDirection == SortDirection.Descending)
                    ApplyOrderByDesc(v => v.Product.Name);
                else
                    ApplyOrderBy(v => v.Product.Name);
            }
            else
            {
                ApplyOrderByDesc(v => v.Product.CreatedAt);
            }

            ApplyPagination(pageSize, pageNumber);
            AsNoTracking();
        }
    }
}