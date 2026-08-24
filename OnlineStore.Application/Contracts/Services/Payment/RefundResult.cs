namespace OnlineStore.Application.Contracts.Services.Payment
{
    public class RefundResult
    {
        public string RefundId { get; set; } = default!;
        public string Status { get; set; } = default!;

        public RefundResult(string refundId, string status)
        {
            RefundId = refundId;
            Status = status;
        }
    }
}