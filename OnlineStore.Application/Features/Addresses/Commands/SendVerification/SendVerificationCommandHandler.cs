using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Contracts.Services.Verification;
using OnlineStore.Domain.Entities.Identity;

namespace OnlineStore.Application.Features.Addresses.Commands.SendVerification;

public class SendVerificationCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IPhoneVerificationService phoneVerificationService,
    ILogger<SendVerificationCommandHandler> logger)
    : IRequestHandler<SendVerificationCommand, Unit>
{
    public async Task<Unit> Handle(
        SendVerificationCommand request,
        CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId
          ?? throw new UnauthorizedException("User must be authenticated.");

        logger.LogInformation(
            "Sending phone verification code for Address {AddressId} and User {UserId}",
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

        if (address.IsPhoneVerified)
        {
            throw new BadRequestException(
                "Phone number is already verified.");
        }

        await phoneVerificationService.SendOtpAsync(
            address.PhoneNumber,
            cancellationToken);

        logger.LogInformation(
            "Verification code sent successfully to {PhoneNumber}",
            address.PhoneNumber);
        return Unit.Value;
    }
}