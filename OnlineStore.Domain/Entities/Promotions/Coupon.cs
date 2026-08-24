using OnlineStore.Domain.Entities.BaseEntities;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Domain.Entities.Promotions
{
    public class Coupon : SoftDeletableEntity
    {
        public string Code { get; set; } = default!;
        public DiscountType Type { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal MinOrderAmount { get; set; }
        public int UsedCount { get; set; } = 0;
        public int? UsageLimit { get; set; }
        // if null means no limit
        public DateTime? StartsAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsActive { get; set; } = true;



        public decimal CalculateDiscount(decimal orderAmount)
        {
            decimal discount = Type switch
            {
                DiscountType.Percentage => orderAmount * DiscountValue / 100m,
                DiscountType.FixedAmount => DiscountValue,
                _ => 0m
            };

            return Math.Min(discount, orderAmount);
        }
        public bool IsValid(decimal orderAmount)
        {
            var now = DateTime.UtcNow;

            if (!IsActive)
                return false;

            if (StartsAt.HasValue && StartsAt > now)
                return false;

            if (ExpiresAt.HasValue && ExpiresAt < now)
                return false;

            if (UsageLimit.HasValue && UsedCount >= UsageLimit.Value)
                return false;

            if (orderAmount < MinOrderAmount)
                return false;

            return true;
        }
    }
}