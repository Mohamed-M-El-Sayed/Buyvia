namespace OnlineStore.Application.Contracts.Services.Payment
{
    public record PaymentIntentResult(
        string PaymentIntentId,
        string ClientSecret
    );
}
