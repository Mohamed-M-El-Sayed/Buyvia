namespace OnlineStore.Infrastructure.Services.Payment
{
    public static class StripeEvents
    {
        public const string PaymentSucceeded = "payment_intent.succeeded";
        public const string PaymentFailed = "payment_intent.payment_failed";
        public const string PaymentRequiresAction = "payment_intent.requires_action";
        public const string PaymentProcessing = "payment_intent.processing";
        public const string PaymentCanceled = "payment_intent.canceled";
    }
}
