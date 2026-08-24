using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductVariants.Specifications
{
    public class VariantWithImagesSpecification : BaseSpecification<ProductVariant>
    {
        public VariantWithImagesSpecification(int variantId)
        {
            Criteria = v => v.Id == variantId;
            ApplyInclude(v => v.Images.OrderBy(i => i.DisplayOrder));

        }
    }
}
