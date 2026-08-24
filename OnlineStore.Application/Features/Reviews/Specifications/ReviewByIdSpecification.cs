using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Application.Features.Reviews.Specifications
{
    public class ReviewByIdSpecification : BaseSpecification<Review>
    {
        public ReviewByIdSpecification(int id)
        {
            Criteria = r => r.Id == id;

            //ApplyInclude(r => r.User);
            ApplyInclude(r => r.User);
            ApplyInclude(r => r.PurchasedVariant);
            ApplyInclude("PurchasedVariant.Options");

        }
    }
}
