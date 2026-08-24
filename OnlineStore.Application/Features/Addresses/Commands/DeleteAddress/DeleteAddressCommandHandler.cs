using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Identity;

namespace OnlineStore.Application.Features.Addresses.Commands.DeleteAddress
{
    public class DeleteAddressCommandHandler(IUnitOfWork unitOfWork,
        ILogger<DeleteAddressCommandHandler> logger,
        ICurrentUserService currentUserService) : IRequestHandler<DeleteAddressCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId
                ?? throw new UnauthorizedException("User must be authenticated.");

            logger.LogInformation(
                 "Deleting address {AddressId} for user {UserId}.",
                 request.AddressId,
                 userId);
            var address = await unitOfWork.Repository<UserAddress>()
            .FindAsync(a => a.Id == request.AddressId && a.UserId == userId)
            ?? throw new NotFoundException(
                nameof(UserAddress),
                request.AddressId.ToString());
            unitOfWork.Repository<UserAddress>().Delete(address);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
