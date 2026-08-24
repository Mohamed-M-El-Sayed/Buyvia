using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Contracts.Services.Verification;
using OnlineStore.Domain.Entities.Identity;

namespace OnlineStore.Application.Features.Addresses.Commands.VerifyPhone;

public class VerifyPhoneCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IPhoneVerificationService phoneVerificationService,
    ILogger<VerifyPhoneCommandHandler> logger)
    : IRequestHandler<VerifyPhoneCommand, Unit>
{
    public async Task<Unit> Handle(
        VerifyPhoneCommand request,
        CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId
            ?? throw new UnauthorizedException("User must be authenticated.");

        logger.LogInformation(
            "Verifying phone number for Address {AddressId} and User {UserId}.",
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

        var isVerified = await phoneVerificationService.VerifyOtpAsync(
            address.PhoneNumber,
            request.Otp,
            cancellationToken);

        if (!isVerified)
        {
            logger.LogWarning(
                "Invalid OTP entered for Address {AddressId}.",
                address.Id);

            throw new BadRequestException(
                "Invalid verification code.");
        }

        address.IsPhoneVerified = true;

        await unitOfWork.CompleteAsync(cancellationToken);

        logger.LogInformation(
            "Phone number verified successfully for Address {AddressId}.",
            address.Id);
        return Unit.Value;
    }
}