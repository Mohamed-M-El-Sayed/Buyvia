using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Promotions;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Coupons.Specifications
{
    public class CouponsCountSpecification : BaseSpecification<Coupon>
    {
        public CouponsCountSpecification(
            string? search,
            DiscountType? type, bool? isActive)
        {
            Criteria = c =>
                (string.IsNullOrEmpty(search) || c.Code.Contains(search)) &&
                (!type.HasValue || c.Type == type.Value) &&
                (!isActive.HasValue || c.IsActive == isActive.Value);
        }
    }
}
