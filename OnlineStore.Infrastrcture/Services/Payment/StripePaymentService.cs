using Microsoft.Extensions.Options;
using OnlineStore.Application.Contracts.Services.Payment;
using Stripe;
namespace OnlineStore.Infrastructure.Services.Payment
{
    public class StripePaymentService : IPaymentService
    {
        private readonly StripeSettings stripeSettings;
        public StripePaymentService(IOptions<StripeSettings> opts)
        {
            stripeSettings = opts.Value;
            StripeConfiguration.ApiKey = stripeSettings.SecretKey;
        }
        public async Task<PaymentIntentResult> CreatePaymentIntentAsync(decimal amount, int orderId, string currency = "egp", CancellationToken cancellationToken = default)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100),
                Currency = currency,
                Metadata = new Dictionary<string, string>
                {
                    ["orderId"] = orderId.ToString()
                },
                PaymentMethodTypes = new List<string> { "card" },

                //AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                //{
                //    Enabled = true
                //}
            };
            var requestOptions = new RequestOptions
            {
                IdempotencyKey = $"payment-intent-order-{orderId}"
            };
            var service = new PaymentIntentService();
            var intent = await service.CreateAsync(options, requestOptions, cancellationToken: cancellationToken);
            return new PaymentIntentResult(intent.Id, intent.ClientSecret);
        }
        public PaymentEventType ParseWebhookEvent(string payload, string stripeSignature)
        {
            var stripeEvent = EventUtility.ConstructEvent(payload, stripeSignature, stripeSettings.WebhookSecret);
            return stripeEvent.Type switch
            {
                StripeEvents.PaymentSucceeded => PaymentEventType.Succeeded,
                StripeEvents.PaymentFailed => PaymentEventType.Failed,
                StripeEvents.PaymentRequiresAction => PaymentEventType.RequiresAction,
                StripeEvents.PaymentProcessing => PaymentEventType.Processing,
                StripeEvents.PaymentCanceled => PaymentEventType.Canceled,
                _ => PaymentEventType.Unknown
            };

        }
        public async Task<RefundResult> RefundAsync(string paymentIntentId, CancellationToken cancellationToken = default)
        {
            var refundService = new RefundService();
            var refund = await refundService.CreateAsync(
                new RefundCreateOptions
                {
                    PaymentIntent = paymentIntentId,
                }, cancellationToken: cancellationToken);
            return new RefundResult(
                refund.Id,
                refund.Status);


        }
    }
}

