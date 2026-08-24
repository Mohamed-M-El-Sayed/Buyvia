namespace OnlineStore.Application.Features.Discounts.Dtos
{
    public class DiscountDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Type { get; set; } = default!;
        public decimal Value { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public DateTime? StartsAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        // Indicates whether the discount is manually enabled by the administrator.
        public bool IsEnabled { get; set; }
        public bool IsCurrentlyActive { get; set; }
    }
}
