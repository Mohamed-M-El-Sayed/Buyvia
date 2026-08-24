using System.Text.Json.Serialization;
using MediatR;

namespace OnlineStore.Application.Features.Addresses.Commands.VerifyPhone
{
    public class VerifyPhoneCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int AddressId { get; set; }
        public string Otp { get; set; } = default!;
    }
}
