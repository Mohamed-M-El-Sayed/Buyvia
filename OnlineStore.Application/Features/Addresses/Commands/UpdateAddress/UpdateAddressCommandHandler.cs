using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Identity;

namespace OnlineStore.Application.Features.Addresses.Commands.UpdateAddress;

public class UpdateAddressCommandHandler(
    IUnitOfWork unitOfWork,
    ILogger<UpdateAddressCommandHandler> logger,
    IMapper mapper,
    ICurrentUserService currentUserService)
    : IRequestHandler<UpdateAddressCommand, Unit>
{
    public async Task<Unit> Handle(
        UpdateAddressCommand request,
        CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId
            ?? throw new UnauthorizedException("User must be authenticated.");
        logger.LogInformation(
            "Updating address {AddressId} for user {UserId}.",
            request.AddressId,
            userId);

        var address = await unitOfWork.Repository<UserAddress>()
            .FindAsync(
                a => a.UserId == userId &&
                     a.Id == request.AddressId,
                cancellationToken)
            ?? throw new NotFoundException(
                nameof(UserAddress),
                request.AddressId.ToString());

        var phoneChanged = address.PhoneNumber != request.PhoneNumber;

        mapper.Map(request, address);

        if (phoneChanged)
        {

            var verifiedAddress = await unitOfWork.Repository<UserAddress>()
                .FindAsync(
                    a => a.UserId == userId &&
                         a.PhoneNumber == request.PhoneNumber &&
                         a.IsPhoneVerified,
                    cancellationToken);

            address.IsPhoneVerified = verifiedAddress is not null;
        }
        unitOfWork.Repository<UserAddress>().Update(address);

        await unitOfWork.CompleteAsync(cancellationToken);

        logger.LogInformation(
            "Address {AddressId} updated successfully.",
            address.Id);

        return Unit.Value;
    }
}