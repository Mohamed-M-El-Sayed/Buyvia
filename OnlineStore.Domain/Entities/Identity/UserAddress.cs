using OnlineStore.Domain.Entities.BaseEntities;

namespace OnlineStore.Domain.Entities.Identity
{
    public class UserAddress : BaseEntity
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public bool IsPhoneVerified { get; set; }
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Country { get; set; } = default!;
        public bool IsDefault { get; set; }
        public ApplicationUser User { get; set; } = default!;
    }
}
