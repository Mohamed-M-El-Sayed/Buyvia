using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Orders;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Orders.Specifications
{
    public class PendingExpiredOrdersSpecification : BaseSpecification<Order>
    {
        public PendingExpiredOrdersSpecification()
        {
            Criteria = o =>
                o.Status == OrderStatus.Pending &&
                o.ExpireAt <= DateTime.UtcNow &&
                o.Payment.Method == PaymentMethod.Card;

            ApplyInclude(order => order.Items);
            ApplyInclude(order => order.Payment!);
            ApplyInclude("Items.ProductVariant");
        }
    }
}
