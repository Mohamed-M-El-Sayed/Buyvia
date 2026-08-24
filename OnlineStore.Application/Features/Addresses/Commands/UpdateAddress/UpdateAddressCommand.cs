using System.Text.Json.Serialization;
using MediatR;

namespace OnlineStore.Application.Features.Addresses.Commands.UpdateAddress
{
    public class UpdateAddressCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int AddressId { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Street { get; set; } = default!;
    }
}
