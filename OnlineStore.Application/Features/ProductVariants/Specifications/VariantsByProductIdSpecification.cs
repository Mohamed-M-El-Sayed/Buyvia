using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductVariants.Specifications
{
    public class VariantsByProductIdSpecification : BaseSpecification<ProductVariant>
    {
        public VariantsByProductIdSpecification(int porductId)
        {
            Criteria = v => v.ProductId == porductId;
            ApplyInclude(v => v.Options);
            ApplyInclude(v => v.Discount!);
            ApplyInclude(v => v.Product);
            ApplyInclude("Options.Value");
            ApplyInclude("Options.Option");
        }
        public VariantsByProductIdSpecification(int productId, List<int>? variantIds = null)
        {
            Criteria = v =>
                v.ProductId == productId &&
                (variantIds == null || variantIds.Contains(v.Id));
        }

    }
}
