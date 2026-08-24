namespace OnlineStore.Application.Features.Wishlists.Dtos
{
    public class WishlistItemDto
    {
        public int WishlistItemId { get; set; }

        public int VariantId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;
        public string VariantName { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;

        public decimal OriginalPrice { get; set; }

        public decimal FinalPrice { get; set; }

        public bool HasDiscount { get; set; }

        public bool InStock { get; set; }
        public string? MainImageUrl { get; set; }

    }
}
