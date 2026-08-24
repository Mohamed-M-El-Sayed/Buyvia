using OnlineStore.Domain.Entities.BaseEntities;
using OnlineStore.Domain.Entities.Identity;
using OnlineStore.Domain.Entities.Promotions;

namespace OnlineStore.Domain.Entities.ShoppingCart
{
    public class Cart : BaseEntity
    {
        public Guid UserId { get; set; } = default!;
        //public decimal TotalPrice { get; set; }
        //public decimal DiscountAmount { get; set; }
        //public string? CouponCode { get; set; }
        public int? CouponId { get; set; }
        public Coupon? Coupon { get; set; }
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
        public ApplicationUser User { get; set; } = default!;
        public decimal SubTotal =>
         Items.Sum(i => i.UnitPrice * i.Quantity);
    }
}