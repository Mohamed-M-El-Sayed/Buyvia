using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Coupons.Dtos
{
    public class CouponDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public DiscountType Type { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal? MinOrderAmount { get; set; }
        public int? UsageLimit { get; set; }
        public int UsedCount { get; set; }
        public bool IsActive { get; set; }
        public DateTime? StartsAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
