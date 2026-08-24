using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductVariants.Specifications
{
    public class VariantWithDetailsSpecification : BaseSpecification<ProductVariant>
    {
        public VariantWithDetailsSpecification(int variantId)
        {
            Criteria = v => v.Id == variantId;
            ApplyInclude(v => v.Images);
            ApplyInclude(v => v.Discount!);
            ApplyInclude("Options.Value");
        }
    }
}
