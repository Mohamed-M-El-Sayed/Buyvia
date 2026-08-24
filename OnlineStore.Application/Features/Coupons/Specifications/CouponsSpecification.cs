using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Promotions;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Coupons.Specifications;

public class CouponsSpecification : BaseSpecification<Coupon>
{
    public CouponsSpecification(
        string? search,
        DiscountType? type,
        bool? isActive,
        int pageNumber,
        int pageSize)
    {
        Criteria = c =>
            (string.IsNullOrEmpty(search) ||
             c.Code.Contains(search)) &&

            (!type.HasValue ||
             c.Type == type.Value) &&

            (!isActive.HasValue ||
             c.IsActive == isActive.Value);
        ApplyPagination(pageSize, pageNumber);
    }
}