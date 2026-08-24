using System.Security.Claims;
using OnlineStore.Application.Features.Auth.Dtos;
using OnlineStore.Domain.Entities.Identity;

namespace OnlineStore.Application.Contracts.Services.Authentication
{
    public interface ITokenService
    {
        public Task<AccessTokenDto> GenerateAccessTokenAsync(ApplicationUser user);

        public RefreshToken GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
