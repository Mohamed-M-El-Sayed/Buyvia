namespace OnlineStore.Domain.Enums
{
    public enum OrderStatus
    {
        Pending,
        PaymentReceived,
        Processing,
        Shipped,
        Delivered,
        Cancelled,
        Refunded,
        Expired
    }

}
