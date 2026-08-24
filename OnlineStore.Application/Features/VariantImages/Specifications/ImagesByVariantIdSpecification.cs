using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.VariantImages.Specifications
{
    public class ImagesByVariantIdSpecification : BaseSpecification<ProductImage>
    {
        public ImagesByVariantIdSpecification(int variantId)
        {
            Criteria = v => v.ProductVariantId == variantId;
        }
    }
}
