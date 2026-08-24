using OnlineStore.Application.Features.Auth.Dtos;

namespace OnlineStore.Application.Contracts.Services.Authentication
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);
        Task<string> ResendConfirmationEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<AuthResponseDto> LoginAsync(string email, string password,
          CancellationToken cancellationToken = default);

        Task ConfirmEmailAsync(string userId, string encodedToken, CancellationToken cancellationToken = default);

        Task<AuthResponseDto> RefreshTokenAsync(string accessToken,
            string refreshToken,
            CancellationToken cancellationToken = default);

        Task<string> ForgotPasswordAsync(string email, CancellationToken cancellationToken = default);
        Task<string> ResetPasswordAsync(string email, string token, string newPassword, CancellationToken cancellationToken = default);
        Task<UserDto> GetCurrentUserAsync(CancellationToken cancellationToken = default);
        Task LogoutAsync(CancellationToken cancellationToken = default);
        //Task<string> AssignAdminAsync(Guid userId, CancellationToken cancellationToken = default);
        //Task<string> UnassignAdminAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
