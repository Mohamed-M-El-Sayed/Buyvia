using MediatR;

namespace OnlineStore.Application.Features.Addresses.Commands.DeleteAddress
{
    public class DeleteAddressCommand(int addressId) : IRequest<Unit>
    {
        public int AddressId { get; } = addressId;
    }
}
