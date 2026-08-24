using OnlineStore.Domain.Entities.BaseEntities;

namespace OnlineStore.Domain.Entities.Products
{
    public class VariantOption : BaseEntity
    {
        public int VariantId { get; set; }
        public int OptionId { get; set; }
        public int OptionValueId { get; set; }

        public ProductVariant Variant { get; set; } = default!;
        public ProductOption Option { get; set; } = default!;
        public ProductOptionValue Value { get; set; } = default!;
    }
}
