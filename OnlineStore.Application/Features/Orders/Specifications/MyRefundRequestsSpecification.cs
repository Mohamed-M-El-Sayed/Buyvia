using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Orders;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Orders.Specifications
{
    public class MyRefundRequestsSpecification
        : BaseSpecification<RefundRequest>
    {
        public MyRefundRequestsSpecification(
            Guid userId,
            RefundRequestStatus? status,
            int pageIndex = 1,
            int pageSize = 50,
            bool isPagingEnabled = true)
        {
            Criteria = r =>
                r.UserId == userId &&
                (!status.HasValue || r.Status == status.Value);

            if (isPagingEnabled)
            {
                ApplyPagination(pageSize, pageIndex);
                ApplyOrderByDesc(r => r.CreatedAt);
            }
        }
    }
}