using MediatR;

namespace OnlineStore.Application.Features.Auth.Commands.UpdateProfile
{
    public class UpdateProfileCommand : IRequest<Unit>
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? PictureUrl { get; set; }
    }
}