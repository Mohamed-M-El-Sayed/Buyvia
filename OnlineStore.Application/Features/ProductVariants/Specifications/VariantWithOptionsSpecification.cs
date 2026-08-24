using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductVariants.Specifications
{
    public class VariantWithOptionsSpecification : BaseSpecification<ProductVariant>
    {
        public VariantWithOptionsSpecification(int productId)
        {
            Criteria = v => v.ProductId == productId;
            ApplyInclude(v => v.Options);
        }

    }
}
