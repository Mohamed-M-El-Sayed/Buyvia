using MediatR;
using OnlineStore.Application.Features.Auth.Dtos;

namespace OnlineStore.Application.Features.Auth.Commands.Register
{
    public class RegisterCommand : IRequest<RegisterResponseDto>
    {
        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string Password { get; set; } = default!;

        public string ConfirmPassword { get; set; } = default!;
    }
}
