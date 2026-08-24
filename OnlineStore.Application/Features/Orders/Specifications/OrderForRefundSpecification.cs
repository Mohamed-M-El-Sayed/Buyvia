using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Application.Features.Orders.Dtos
{
    public class OrderForRefundSpecification : BaseSpecification<Order>
    {
        public OrderForRefundSpecification(int orderId, Guid userId)
            : base(x => x.Id == orderId && x.UserId == userId)
        {
            ApplyInclude(x => x.Payment);
            ApplyInclude(x => x.RefundRequests);
        }
    }
}
