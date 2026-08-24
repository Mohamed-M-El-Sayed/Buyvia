using OnlineStore.Application.Common.Specifications;
using OnlineStore.Application.Features.Discounts.Queries.GetAllDiscounts;
using OnlineStore.Domain.Entities.Promotions;

namespace OnlineStore.Application.Features.Discounts.Specifications
{
    public class DiscountsSpecification : BaseSpecification<Discount>
    {
        public DiscountsSpecification(
              GetAllDiscountsQuery request,
              bool applyPaging = true,
              bool applySorting = true)
        {
            var now = DateTime.UtcNow;

            Criteria = d =>
                (!request.IsEnabled.HasValue ||
                    d.IsEnabled == request.IsEnabled.Value)

                &&

                (!request.Type.HasValue ||
                    d.Type == request.Type.Value)

                &&

                (
                    !request.IsCurrentlyActive.HasValue ||

                    (request.IsCurrentlyActive.Value
                        ? d.IsEnabled &&
                          (!d.StartsAt.HasValue || d.StartsAt <= now) &&
                          (!d.ExpiresAt.HasValue || d.ExpiresAt >= now)

                        : !d.IsEnabled ||
                          (d.StartsAt.HasValue && d.StartsAt > now) ||
                          (d.ExpiresAt.HasValue && d.ExpiresAt < now))
                );

            if (applySorting)
            {
                ApplySorting(request);
            }

            if (applyPaging)
            {
                ApplyPagination(request.PageSize, request.PageNumber);
            }

            AsNoTracking();
        }

        private void ApplySorting(GetAllDiscountsQuery request)
        {
            switch (request.SortBy)
            {
                case DiscountSortField.Name:
                    if (request.SortDescending)
                        ApplyOrderByDesc(x => x.Name);
                    else
                        ApplyOrderBy(x => x.Name);
                    break;

                case DiscountSortField.Value:
                    if (request.SortDescending)
                        ApplyOrderByDesc(x => x.Value);
                    else
                        ApplyOrderBy(x => x.Value);
                    break;

                case DiscountSortField.StartsAt:
                    if (request.SortDescending)
                        ApplyOrderByDesc(x => x.StartsAt!);
                    else
                        ApplyOrderBy(x => x.StartsAt!);
                    break;

                case DiscountSortField.ExpiresAt:
                    if (request.SortDescending)
                        ApplyOrderByDesc(x => x.ExpiresAt!);
                    else
                        ApplyOrderBy(x => x.ExpiresAt!);
                    break;

                case DiscountSortField.CreatedAt:
                default:
                    if (request.SortDescending)
                        ApplyOrderByDesc(x => x.CreatedAt);
                    else
                        ApplyOrderBy(x => x.CreatedAt);
                    break;
            }
        }
    }
}
