using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.Products.Specifications
{
    public class ProductForPublishSpecification : BaseSpecification<Product>
    {
        public ProductForPublishSpecification(int productId)
        {
            Criteria = p => p.Id == productId;
            ApplyInclude(p => p.Variants);
            ApplyInclude("Variants.Images");
        }
    }
}


// ProductWithVariantImagesSpecification