using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Application.Features.Orders.Specifications
{
    public class OrdersByUserSpecification : BaseSpecification<Order>
    {
        public OrdersByUserSpecification(
         Guid userId,
         bool applyPagination = false,
         int pageNumber = 1,
         int pageSize = 10)
        {
            Criteria = o => o.UserId == userId;
            if (applyPagination)
            {
                ApplyInclude(o => o.Items);
                ApplyPagination(pageSize, pageNumber);
                OrderByDescending = o => o.CreatedAt;
            }
        }
    }
}
