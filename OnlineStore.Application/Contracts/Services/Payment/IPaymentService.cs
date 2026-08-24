namespace OnlineStore.Application.Contracts.Services.Payment
{
    public interface IPaymentService
    {
        Task<PaymentIntentResult> CreatePaymentIntentAsync(
            decimal amount,
            int orderId,
            string currency = "egp",
            CancellationToken cancellationToken = default);
        PaymentEventType ParseWebhookEvent(string payload, string stripeSignature);

        Task<RefundResult> RefundAsync(string paymentIntentId, CancellationToken cancellationToken = default);
    }
}
