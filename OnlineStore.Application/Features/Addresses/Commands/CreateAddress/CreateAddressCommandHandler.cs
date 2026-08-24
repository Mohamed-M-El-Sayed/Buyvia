using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Identity;

namespace OnlineStore.Application.Features.Addresses.Commands.CreateAddress;

public class CreateAddressCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IMapper mapper,
    ILogger<CreateAddressCommandHandler> logger)
    : IRequestHandler<CreateAddressCommand, int>
{
    public async Task<int> Handle(
        CreateAddressCommand request,
        CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId
          ?? throw new UnauthorizedException("User must be authenticated.");

        logger.LogInformation(
            "Creating address for user {UserId}.",
            userId);
        var address = mapper.Map<UserAddress>(request);

        address.UserId = userId;

        var verifiedAddress = await unitOfWork.Repository<UserAddress>()
            .FindAsync(
                a => a.UserId == userId &&
                     a.PhoneNumber == request.PhoneNumber &&
                     a.IsPhoneVerified,
                cancellationToken);

        address.IsPhoneVerified = verifiedAddress is not null;

        if (request.IsDefault)
        {
            var currentDefaultAddress = await unitOfWork.Repository<UserAddress>()
                .FindAsync(
                    a => a.UserId == userId && a.IsDefault,
                    cancellationToken);

            if (currentDefaultAddress is not null)
            {
                currentDefaultAddress.IsDefault = false;
            }
        }

        await unitOfWork.Repository<UserAddress>()
            .AddAsync(address, cancellationToken);

        await unitOfWork.CompleteAsync(cancellationToken);

        logger.LogInformation(
            "Address {AddressId} created for user {UserId}. PhoneVerified: {IsPhoneVerified}",
            address.Id,
            userId,
            address.IsPhoneVerified);

        return address.Id;
    }
}