namespace OnlineStore.Application.Contracts.Services.Payment
{
    public enum PaymentEventType
    {
        Succeeded,
        Failed,
        RequiresAction,
        Processing,
        Canceled,
        Unknown
    }
}
