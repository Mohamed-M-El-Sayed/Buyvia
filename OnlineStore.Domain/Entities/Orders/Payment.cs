using OnlineStore.Domain.Entities.BaseEntities;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Domain.Entities.Orders
{
    public class Payment : SoftDeletableEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; } = default!;
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public PaymentMethod Method { get; set; }
        public DateTime? PaidAt { get; set; }

        public string? TransactionId { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? RefundId { get; set; }
        public string? FailureReason { get; set; }
    }
}
