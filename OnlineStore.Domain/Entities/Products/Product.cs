using OnlineStore.Domain.Entities.BaseEntities;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Domain.Entities.Products
{

    public class Product : SoftDeletableEntity
    {
        public string Name { get; set; } = default!;
        public string ShortDescription { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int? BrandId { get; set; }
        public ProductBrand Brand { get; set; } = default!;
        public int? CategoryId { get; set; }
        public ProductCategory Category { get; set; } = default!;


        public ProductStatus Status { get; set; }
        // For simple products this is the only variant
        // For products with variants products this is the variant shown first
        public ProductVariant? DefaultVariant =>
           Variants.FirstOrDefault(v => v.IsDefault);

        // IsSimple now determined by presence of per-product Options
        public bool IsSimple => !Options.Any();

        // per-product options
        public ICollection<ProductOption> Options { get; set; } = new List<ProductOption>();

        //public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public override void Delete()
        {
            // delete the product and all its variants (soft delete)
            base.Delete();
            foreach (var variant in Variants)
                variant.Delete();
        }
    }
}
