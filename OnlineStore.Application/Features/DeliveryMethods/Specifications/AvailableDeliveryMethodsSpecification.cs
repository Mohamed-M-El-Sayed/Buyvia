using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Application.Features.DeliveryMethods.Specifications
{
    public class AvailableDeliveryMethodsSpecification
        : BaseSpecification<DeliveryMethod>
    {
        public AvailableDeliveryMethodsSpecification()
        {
            Criteria = x => x.IsActive;
            ApplyOrderBy(x => x.Price);
        }
    }
}
