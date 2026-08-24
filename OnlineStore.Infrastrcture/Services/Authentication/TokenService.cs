using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.Application.Contracts.Services.Authentication;
using OnlineStore.Application.Features.Auth.Dtos;
using OnlineStore.Domain.Entities.Identity;

namespace OnlineStore.Infrastructure.Services.Authentication
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwt;
        private readonly ILogger<TokenService> _logger;
        public TokenService(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwtSettings, ILogger<TokenService> logger)
        {
            _userManager = userManager;
            _jwt = jwtSettings.Value;
            _logger = logger;
        }
        public async Task<AccessTokenDto> GenerateAccessTokenAsync(ApplicationUser user)
        {

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email , user.Email!) ,
                new(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()) ,
                new(JwtRegisteredClaimNames.Name , user.UserName!)
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );
            var expiresAt = DateTime.UtcNow.AddMinutes(_jwt.AccessTokenExpirationMinutes);
            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: signingCredentials
            );
            return new() { Token = new JwtSecurityTokenHandler().WriteToken(token), ExpiresAt = expiresAt };
        }
        public RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken()
            {

                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiresAt = DateTime.UtcNow.AddDays(_jwt.RefreshTokenExpirationDays)
            };
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwt.Issuer,

                ValidateAudience = true,
                ValidAudience = _jwt.Audience,

                ValidateLifetime = false,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwt.Key)
                ),

                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(
                    token,
                    tokenValidationParameters,
                    out var securityToken);

                if (securityToken is not JwtSecurityToken jwtToken ||
                    !jwtToken.Header.Alg.Equals(
                        SecurityAlgorithms.HmacSha256,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }

                return principal;
            }
            catch (SecurityTokenException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Invalid access token supplied for refresh.");

                return null;
            }
        }
    }
}


