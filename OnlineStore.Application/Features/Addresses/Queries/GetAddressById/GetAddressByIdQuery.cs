using MediatR;
using OnlineStore.Application.Features.Addresses.Dtos;

namespace OnlineStore.Application.Features.Addresses.Queries.GetAddressById
{
    public class GetAddressByIdQuery(int addressId) : IRequest<AddressDto>
    {
        public int AddressId { get; } = addressId;
    }
}
