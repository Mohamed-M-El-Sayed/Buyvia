using MediatR;
using OnlineStore.Application.Features.Addresses.Dtos;

namespace OnlineStore.Application.Features.Addresses.Queries.GetMyAddresses
{
    public class GetMyAddressesQuery : IRequest<List<AddressDto>>
    {

    }
}
