using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Addresses.Dtos;
using OnlineStore.Domain.Entities.Identity;

namespace OnlineStore.Application.Features.Addresses.Queries.GetAddressById
{


    public class GetAddressByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IMapper mapper,
        ILogger<GetAddressByIdQueryHandler> logger)
        : IRequestHandler<GetAddressByIdQuery, AddressDto>
    {
        public async Task<AddressDto> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId
                ?? throw new UnauthorizedException("User must be authenticated.");
            logger.LogInformation(
                        "Getting address {AddressId} for user {UserId}.",
                        request.AddressId,
                        userId);
            var address = await unitOfWork.Repository<UserAddress>()
                .FindAsync(a => a.Id == request.AddressId && a.UserId == userId)
                ?? throw new NotFoundException(nameof(UserAddress), request.AddressId.ToString());
            return mapper.Map<AddressDto>(address);

        }
    }
}
