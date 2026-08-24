using OnlineStore.Domain.Entities.BaseEntities;

namespace OnlineStore.Domain.Entities.Products
{
    public class ProductBrand : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string LogoUrl { get; set; } = default!;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
