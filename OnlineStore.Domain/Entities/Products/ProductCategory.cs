using OnlineStore.Domain.Entities.BaseEntities;

namespace OnlineStore.Domain.Entities.Products
{
    public class ProductCategory : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; } = default!;
        public int? ParentId { get; set; }
        public bool IsTopLevel => ParentId == null;
        public ProductCategory? Parent { get; set; }
        public ICollection<ProductCategory> SubCategories { get; set; } = new List<ProductCategory>();






    }
}
