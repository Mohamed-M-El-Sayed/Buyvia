using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Application.Features.Payments.Specifications
{
    public class PaymentByIntentIdSpecification : BaseSpecification<Payment>
    {
        public PaymentByIntentIdSpecification(string paymentIntentId)
        {
            Criteria = p => p.PaymentIntentId == paymentIntentId;

            ApplyInclude(p => p.Order);
            ApplyInclude("Order.Items");
            ApplyInclude("Order.Items.ProductVariant");
        }
    }
}