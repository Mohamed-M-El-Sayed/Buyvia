namespace OnlineStore.Application.Features.Payments.Dtos
{
    public class PaymentIntentDto
    {
        public string PaymentIntentId { get; set; } = default!;
        public string ClientSecret { get; set; } = default!;
    }
}
