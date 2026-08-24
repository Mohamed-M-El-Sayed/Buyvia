using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductVariants.Specifications
{
    public class VariantWithSiblingsSpecification : BaseSpecification<ProductVariant>
    {
        public VariantWithSiblingsSpecification(int variantId)
        {
            Criteria = v => v.Id == variantId;
            ApplyInclude(v => v.Product);
            ApplyInclude("Product.Variants");
        }
    }
}
