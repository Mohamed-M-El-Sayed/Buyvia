using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Application.Features.Orders.Specifications
{
    public class RefundRequestForApprovalSpecification : BaseSpecification<RefundRequest>
    {
        public RefundRequestForApprovalSpecification(int refundRequestId)

        {
            Criteria = x => x.Id == refundRequestId;
            ApplyInclude(x => x.Order);
            ApplyInclude(x => x.Order.Payment);
            ApplyInclude(x => x.Order.Items);
            ApplyInclude(x => x.Order.Coupon!);
            ApplyInclude("Order.Items.ProductVariant");
        }
    }
}
