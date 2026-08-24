using OnlineStore.Domain.Entities.BaseEntities;

namespace OnlineStore.Domain.Entities.Products
{
    public class ProductOption : BaseEntity
    {

        public int ProductId { get; set; }
        public string Name { get; set; } = default!;
        public Product Product { get; set; } = default!;
        public ICollection<ProductOptionValue> Values { get; set; } = new List<ProductOptionValue>();
    }
}
