using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Application.Features.Orders.Specifications
{
    public class OrderWithPaymentSpecification : BaseSpecification<Order>
    {
        public OrderWithPaymentSpecification(int orderID)
        {
            Criteria = o => o.Id == orderID;
            ApplyInclude(o => o.Payment!);
        }
    }


}
