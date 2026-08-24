namespace OnlineStore.Application.Features.Reviews.Dtos
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int? PurchasedVariantId { get; set; }
        public string? PurchasedVariantName { get; set; }  // "Red - XL"
        public string ReviewerName { get; set; } = default!;
        public int Rating { get; set; }
        public string? Title { get; set; }
        public string Comment { get; set; } = default!;
        // public bool IsVerifiedPurchase { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}