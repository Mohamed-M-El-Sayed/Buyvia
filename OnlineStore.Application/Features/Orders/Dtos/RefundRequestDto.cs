using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Orders.Dtos
{
    public class RefundRequestDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public string? Reason { get; set; }
        public RefundRequestStatus Status { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string? RejectionReason { get; set; }
    }
}
