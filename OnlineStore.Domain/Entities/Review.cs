using OnlineStore.Domain.Entities.BaseEntities;
using OnlineStore.Domain.Entities.Identity;
using OnlineStore.Domain.Entities.Products;
namespace OnlineStore.Domain.Entities
{
    public class Review : SoftDeletableEntity
    {
        public Guid UserId { get; set; }
        public int Rating { get; set; }
        public string? Title { get; set; }
        public string Comment { get; set; } = default!;
        // public bool IsVerifiedPurchase { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;
        public int PurchasedVariantId { get; set; }
        public ProductVariant PurchasedVariant { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;
    }
}