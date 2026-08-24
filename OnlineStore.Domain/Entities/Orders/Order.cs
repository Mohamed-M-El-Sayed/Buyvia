using OnlineStore.Domain.Entities.BaseEntities;
using OnlineStore.Domain.Entities.Identity;
using OnlineStore.Domain.Entities.Promotions;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Domain.Entities.Orders
{
    public class Order : SoftDeletableEntity
    {
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public decimal Subtotal { get; set; }        // sum of item FinalPrices
        public decimal ItemsDiscount { get; set; }
        public string? CouponCode { get; set; }
        public int? CouponId { get; set; }
        public Coupon? Coupon { get; set; }
        public decimal CouponDiscount { get; set; }  // from coupon
        public decimal DeliveryFee { get; set; }     // from DeliveryMethod
        public decimal Total { get; set; }   // Subtotal - CouponDiscount + DeliveryFee
        public OrderAddress ShippingAddress { get; set; } = default!;
        public int DeliveryMethodId { get; set; }
        public DateTime ExpireAt { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; } = default!;
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public Payment Payment { get; set; } = default!;
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = default!;
        public ICollection<RefundRequest> RefundRequests { get; set; }
            = new List<RefundRequest>();
    }
}
