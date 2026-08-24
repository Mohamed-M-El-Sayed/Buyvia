using MediatR;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Services.Authentication;

namespace OnlineStore.Application.Features.Auth.Commands.UpdateProfile
{
    public class UpdateProfileCommandHandler(
        IUserService userService,
        ICurrentUserService currentUserService)
        : IRequestHandler<UpdateProfileCommand, Unit>
    {
        public async Task<Unit> Handle(
            UpdateProfileCommand request,
            CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId
                ?? throw new UnauthorizedException();

            var user = await userService.FindByIdAsync(
                userId,
                cancellationToken)
                ?? throw new NotFoundException(
                    "User",
                    userId.ToString());

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.ProfilePictureUrl = request.PictureUrl;

            await userService.UpdateAsync(user);
            return Unit.Value;
        }
    }
}