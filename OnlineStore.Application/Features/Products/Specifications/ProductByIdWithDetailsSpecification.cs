using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Products.Specifications
{
    public class ProductDetailsByIdSpecification : BaseSpecification<Product>
    {

        public ProductDetailsByIdSpecification(int productId)
        {
            Criteria = product => product.Id == productId && product.Status == Domain.Enums.ProductStatus.Published;
            ApplyInclude(p => p.Variants.Where(v => v.IsActive));
            ApplyInclude(p => p.Options);
            ApplyInclude(p => p.Brand);
            ApplyInclude(p => p.Category);
            ApplyInclude("Options.Values");
            ApplyInclude("Variants.Options");
            ApplyInclude("Variants.Options.Value");
            ApplyInclude("Variants.Discount");
            ApplyInclude("Variants.Images");
            AsNoTracking();
        }
    }
}
