using OnlineStore.Domain.Entities.BaseEntities;
using OnlineStore.Domain.Entities.Identity;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Domain.Entities.Orders
{
    public class RefundRequest : SoftDeletableEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; } = default!;

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = default!;

        public RefundRequestStatus Status { get; set; } = RefundRequestStatus.Pending;

        public string? Reason { get; set; }

        public decimal Amount { get; set; }

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ReviewedAt { get; set; }

        public Guid? ReviewedById { get; set; }
        public ApplicationUser? ReviewedBy { get; set; }

        public string? RejectionReason { get; set; }
    }
}
