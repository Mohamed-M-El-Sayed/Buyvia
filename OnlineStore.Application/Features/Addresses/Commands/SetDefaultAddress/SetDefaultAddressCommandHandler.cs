using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Identity;

namespace OnlineStore.Application.Features.Addresses.Commands.SetDefaultAddress
{
    public class SetDefaultAddressCommandHandler(IUnitOfWork unitOfWork,
        ILogger<SetDefaultAddressCommandHandler> logger,
        ICurrentUserService currentUserService) : IRequestHandler<SetDefaultAddressCommand, Unit>
    {
        public async Task<Unit> Handle(SetDefaultAddressCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId
                ?? throw new UnauthorizedException("User must be authenticated.");

            logger.LogInformation(
                        "Setting address {AddressId} as default for user {UserId}.",
                        request.AddressId,
                        userId);

            var address = await unitOfWork.Repository<UserAddress>()
           .FindAsync(
               a => a.Id == request.AddressId &&
                    a.UserId == userId,
               cancellationToken)
           ?? throw new NotFoundException(
               nameof(UserAddress),
               request.AddressId.ToString());

            if (address.IsDefault)
            {
                return Unit.Value;
            }
            var currentDefaultAddress = await unitOfWork.Repository<UserAddress>()
                .FindAsync(
                    a => a.UserId == userId && a.IsDefault,
                    cancellationToken);
            if (currentDefaultAddress is not null)
            {
                currentDefaultAddress.IsDefault = false;
                await unitOfWork.CompleteAsync(cancellationToken);
            }
            address.IsDefault = true;
            await unitOfWork.CompleteAsync(cancellationToken);
            logger.LogInformation(
             "Address {AddressId} has been set as the default address for user {UserId}.",
             address.Id,
             userId);
            return Unit.Value;
        }
    }
}
