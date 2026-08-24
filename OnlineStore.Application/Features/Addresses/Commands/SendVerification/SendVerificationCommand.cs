using MediatR;

namespace OnlineStore.Application.Features.Addresses.Commands.SendVerification
{
    public class SendVerificationCommand(int addressId) : IRequest<Unit>
    {
        public int AddressId { get; } = addressId;
    }
}
