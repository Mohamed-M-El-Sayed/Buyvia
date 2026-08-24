using OnlineStore.Domain.Entities.BaseEntities;
using OnlineStore.Domain.Entities.Products;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Domain.Entities.Promotions
{
    public class Discount : SoftDeletableEntity
    {
        public string Name { get; set; } = default!;

        public DiscountType Type { get; set; }

        public decimal Value { get; set; }

        public decimal? MaxDiscountAmount { get; set; }
        // used with Percentage
        public DateTime? StartsAt { get; set; }
        public DateTime? ExpiresAt { get; set; }

        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
        public bool IsEnabled { get; set; } = true;

        public bool IsActive()
        {
            if (!IsEnabled) return false;
            var now = DateTime.UtcNow;
            if (StartsAt.HasValue && now < StartsAt.Value) return false;
            if (ExpiresAt.HasValue && now > ExpiresAt.Value) return false;
            return true;
        }

        public decimal CalculateDiscount(decimal price)
        {
            if (!IsActive())
                return 0;

            if (Type == DiscountType.FixedAmount)
                return Value;

            var discount = price * Value / 100;

            if (MaxDiscountAmount.HasValue)
                discount = Math.Min(discount, MaxDiscountAmount.Value);

            return discount;
        }


    }
}
