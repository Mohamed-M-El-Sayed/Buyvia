using MediatR;

namespace OnlineStore.Application.Features.Addresses.Commands.CreateAddress
{
    public class CreateAddressCommand : IRequest<int>
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Country { get; set; } = default!;
        public bool IsDefault { get; set; }
    }
}
