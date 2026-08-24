namespace OnlineStore.Application.Features.Auth.Dtos
{
    public class RegisterRequestDto
    {
        public string FirstName { get; init; } = default!;

        public string LastName { get; init; } = default!;

        public string Email { get; init; } = default!;

        public string Password { get; init; } = default!;
    }
}
