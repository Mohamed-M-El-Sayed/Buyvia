namespace OnlineStore.Application.Features.Addresses.Dtos
{
    public class AddressDto
    {
        public int Id { get; init; }

        public string FullName { get; init; } = default!;

        public string PhoneNumber { get; init; } = default!;

        public string Country { get; init; } = default!;

        public string City { get; init; } = default!;

        public string Street { get; init; } = default!;

        public bool IsDefault { get; init; }

        public bool IsPhoneVerified { get; init; }
    }
}
