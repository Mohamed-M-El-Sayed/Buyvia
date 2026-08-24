using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Identity;

namespace OnlineStore.Application.Features.Addresses.Specifications
{
    public class MyAddressesSpecification : BaseSpecification<UserAddress>
    {
        public MyAddressesSpecification(Guid userId)
        {
            Criteria = address => address.UserId == userId;
            OrderByDescending = address => address.IsDefault;
        }
    }
}
