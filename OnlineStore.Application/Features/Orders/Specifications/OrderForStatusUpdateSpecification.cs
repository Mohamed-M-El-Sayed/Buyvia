using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Application.Features.Orders.Specifications
{
    public class OrderForStatusUpdateSpecification : BaseSpecification<Order>
    {
        public OrderForStatusUpdateSpecification(int orderId, Guid userId)
        {
            Criteria = o => o.Id == orderId && o.UserId == userId;
            ApplyInclude(o => o.Payment!);
            ApplyInclude(o => o.Items);
            ApplyInclude("Items.ProductVariant");
        }
    }
}
