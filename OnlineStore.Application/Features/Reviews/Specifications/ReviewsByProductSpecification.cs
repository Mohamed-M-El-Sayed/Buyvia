using OnlineStore.Application.Common.Specifications;
using OnlineStore.Application.Features.Reviews.Queries.GetReviewsByProduct;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Application.Features.Reviews.Specifications
{
    public class ReviewsByProductSpecification : BaseSpecification<Review>
    {
        public ReviewsByProductSpecification(
            GetReviewsByProductQuery request,
            bool isPaginationEnabled = true)
        {
            Criteria = r =>
                r.ProductId == request.ProductId &&
                (!request.ExactRating.HasValue || r.Rating == request.ExactRating.Value);

            ApplySorting(request.SortBy, request.Descending);

            if (isPaginationEnabled)
            {
                ApplyInclude(r => r.User);
                ApplyInclude(r => r.PurchasedVariant);
                ApplyInclude("PurchasedVariant.Options.Value");


                ApplyPagination(request.PageSize, request.PageNumber);
            }

            AsNoTracking();
        }

        private void ApplySorting(ReviewSortField sortBy, bool descending)
        {
            switch (sortBy)
            {
                case ReviewSortField.CreatedAt:
                    if (descending)
                        ApplyOrderByDesc(r => r.CreatedAt);
                    else
                        ApplyOrderBy(r => r.CreatedAt);
                    break;

                case ReviewSortField.Rating:
                    if (descending)
                        ApplyOrderByDesc(r => r.Rating);
                    else
                        ApplyOrderBy(r => r.Rating);
                    break;
            }
        }
    }
}