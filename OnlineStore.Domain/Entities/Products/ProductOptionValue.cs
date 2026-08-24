using OnlineStore.Domain.Entities.BaseEntities;

namespace OnlineStore.Domain.Entities.Products
{
    public class ProductOptionValue : BaseEntity
    {
        public int OptionId { get; set; }
        public string Value { get; set; } = default!;
        public ProductOption Option { get; set; } = default!;
    }
}
