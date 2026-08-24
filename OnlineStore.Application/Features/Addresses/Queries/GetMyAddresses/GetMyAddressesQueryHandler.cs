using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Addresses.Dtos;
using OnlineStore.Application.Features.Addresses.Specifications;
using OnlineStore.Domain.Entities.Identity;

namespace OnlineStore.Application.Features.Addresses.Queries.GetMyAddresses
{
    public class GetMyAddressesQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetMyAddressesQueryHandler> logger,
        IMapper mapper,
        ICurrentUserService currentUserService) : IRequestHandler<GetMyAddressesQuery, List<AddressDto>>
    {
        public async Task<List<AddressDto>> Handle(GetMyAddressesQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId
              ?? throw new UnauthorizedException("User must be authenticated.");
            logger.LogInformation(
            "Getting addresses for user {UserId}.",
            userId);

            var addresses = await unitOfWork.Repository<UserAddress>()
                .GetAllWithSpecAsync(new MyAddressesSpecification(userId), cancellationToken);
            return mapper.Map<List<AddressDto>>(addresses);
        }
    }
}
