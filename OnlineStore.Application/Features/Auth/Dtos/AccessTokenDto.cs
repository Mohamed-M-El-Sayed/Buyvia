namespace OnlineStore.Application.Features.Auth.Dtos
{
    public class AccessTokenDto
    {
        public string Token { get; init; } = default!;

        public DateTime ExpiresAt { get; init; }
    }
}
