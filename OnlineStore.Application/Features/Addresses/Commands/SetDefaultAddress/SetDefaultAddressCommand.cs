using MediatR;

namespace OnlineStore.Application.Features.Addresses.Commands.SetDefaultAddress
{
    public class SetDefaultAddressCommand(int addressId) : IRequest<Unit>
    {
        public int AddressId { get; } = addressId;
    }
}
