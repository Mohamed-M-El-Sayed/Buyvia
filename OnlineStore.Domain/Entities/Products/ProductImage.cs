using OnlineStore.Domain.Entities.BaseEntities;

namespace OnlineStore.Domain.Entities.Products
{
    public class ProductImage : BaseEntity
    {
        public string ImageUrl { get; set; } = default!;
        public int DisplayOrder { get; set; }
        public bool IsMainImage { get; set; }
        public int ProductVariantId { get; set; } = default!;
    }
}
