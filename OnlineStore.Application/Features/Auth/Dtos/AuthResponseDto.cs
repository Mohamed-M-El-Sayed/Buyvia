namespace OnlineStore.Application.Features.Auth.Dtos
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = default!;
        public DateTime AccessTokenExpiresAt { get; set; }

        public string RefreshToken { get; set; } = default!;
        public DateTime RefreshTokenExpiresAt { get; init; }
        public UserDto User { get; set; } = default!;
    }
}
