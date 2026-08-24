using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Orders;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Orders.Specifications
{
    public class RefundRequestsSpecification : BaseSpecification<RefundRequest>
    {
        public RefundRequestsSpecification(RefundRequestStatus? status, int pageIndex = 1, int pageSize = 50, bool isPagingEnabled = true)
        {
            Criteria = r => !status.HasValue || r.Status == status.Value;
            if (isPagingEnabled)
            {
                ApplyPagination(pageSize, pageIndex);
                ApplyOrderByDesc(r => r.CreatedAt);
            }
            AsNoTracking();
        }
    }
}
