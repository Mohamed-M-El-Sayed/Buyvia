using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Application.Features.DeliveryMethods.Specifications
{
    public class ActiveDeliveryMethodsSpecification : BaseSpecification<DeliveryMethod>
    {
        public ActiveDeliveryMethodsSpecification()
        {
            Criteria = d => d.IsActive;
        }
    }
}
