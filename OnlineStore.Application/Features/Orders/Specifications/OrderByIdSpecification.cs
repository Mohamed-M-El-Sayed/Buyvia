using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Application.Features.Orders.Specifications
{
    public class OrderByIdSpecification : BaseSpecification<Order>
    {
        public OrderByIdSpecification(int orderId, Guid userId)
        {
            Criteria = o => o.UserId == userId && o.Id == orderId;
            ApplyInclude(o => o.Items);
            ApplyInclude(o => o.DeliveryMethod);
        }
    }
}
