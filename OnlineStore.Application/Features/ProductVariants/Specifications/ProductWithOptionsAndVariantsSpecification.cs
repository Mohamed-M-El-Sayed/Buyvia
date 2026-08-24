using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductVariants.Specifications
{
    public class ProductWithOptionsAndVariantsSpecification : BaseSpecification<Product>
    {
        public ProductWithOptionsAndVariantsSpecification(int productId)
        {
            Criteria = p => p.Id == productId;
            ApplyInclude(p => p.Options);
            ApplyInclude("Options.Values");
            ApplyInclude(p => p.Variants);
            ApplyInclude("Variants.Options");

        }

    }


}
