using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Contracts.Services.Authentication;
using OnlineStore.Application.Contracts.Services.Email;
using OnlineStore.Application.Features.Auth.Dtos;
using OnlineStore.Application.Features.Auth.Specifications;
using OnlineStore.Domain.Constants;
using OnlineStore.Domain.Entities.Identity;

namespace OnlineStore.Infrastructure.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ICurrentUserService _currentUserService;
        public AuthService(UserManager<ApplicationUser> userManager,
                           ITokenService tokenService,
                           IUnitOfWork unitOfWork,
                           IEmailService emailService,
                           IConfiguration configuration,
                           ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _configuration = configuration;
            _currentUserService = currentUserService;
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
        {

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
                throw new BadRequestException("Email is already registered.");

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new BadRequestException(
                 string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            await _userManager.AddToRoleAsync(user, Roles.Customer);



            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            // Encode the token to make it safe for use in a URL
            var encodedToken = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(token));
            var confirmationLink = $"{_configuration["AppSettings:BaseUrl"]}api/auth/confirm-email?userId={user.Id}&token={encodedToken}";

            var body = $@"
                <h2>Welcome {user.FirstName}</h2>

                <p>Thank you for registering.</p>

                <p>
                    Please confirm your email by clicking
                    <a href='{confirmationLink}'>here</a>.
                </p>";
            await _emailService.SendEmailAsync(
            user.Email!,
            "Confirm your email",
            body,
            cancellationToken);
            return new RegisterResponseDto
            {

                Message = "Registration successful. Please check your email to verify your account."
            };
        }
        public async Task<string> ResendConfirmationEmailAsync(
    string email,
    CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null || user.EmailConfirmed)
            {
                return "If an account with that email exists and is not yet confirmed, a confirmation link has been sent.";
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var confirmationLink = $"{_configuration["AppSettings:BaseUrl"]}api/auth/confirm-email?userId={user.Id}&token={encodedToken}";

            var body = $@"
                            <h2>Confirm your email</h2>
                            <p>Hello {user.FirstName},</p>
                            <p>
                                Please confirm your email by clicking
                                <a href='{confirmationLink}'>here</a>.
                            </p>";

            await _emailService.SendEmailAsync(user.Email!, "Confirm your email", body, cancellationToken);

            return "If an account with that email exists and is not yet confirmed, a confirmation link has been sent.";
        }


        public async Task ConfirmEmailAsync(string userId, string encodedToken, CancellationToken cancellationToken = default)
        {
            // note valite on isdelted
            var user = await _userManager.FindByIdAsync(userId)
                ?? throw new NotFoundException("User not found.");

            // Decode the token
            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(encodedToken));

            var result = _userManager.ConfirmEmailAsync(user, token);
            if (!result.Result.Succeeded)
            {
                throw new BadRequestException(
                    string.Join(", ", result.Result.Errors.Select(e => e.Description)));
            }
        }

        public async Task<AuthResponseDto> LoginAsync(string email, string password,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new UnauthorizedException("Invalid email or password.");

            if (!user.EmailConfirmed)
                throw new UnauthorizedException("Please confirm your email before logging in.");

            var isValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isValid)
                throw new UnauthorizedException("Invalid email or password.");

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            refreshToken.UserId = user.Id;
            await _unitOfWork.Repository<RefreshToken>()
                    .AddAsync(refreshToken, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return new()
            {
                AccessToken = accessToken.Token,
                AccessTokenExpiresAt = accessToken.ExpiresAt,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiresAt = refreshToken.ExpiresAt,
                User = new()
                {
                    Id = user.Id.ToString(),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email!,
                    Roles = roles,
                    ProfilePictureUrl = $"{_configuration["AppSettings:BaseUrl"]}{user.ProfilePictureUrl ?? "default.png"}",
                }
            };

        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string accessToken, string refreshToken, CancellationToken cancellationToken = default)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken)
                ?? throw new UnauthorizedException("Invalid access token.");

            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedException("Invalid access token claims.");
            var existingToken = await _unitOfWork.Repository<RefreshToken>()
                .FindAsync(rt => rt.Token == refreshToken && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow, cancellationToken)
                ?? throw new UnauthorizedException("Refresh token is invalid or expired.");
            if (existingToken.UserId.ToString() != userId)
                throw new UnauthorizedException(
                    "Refresh token does not match the provided access token.");
            var user = await _userManager.FindByIdAsync(userId.ToString())
                ?? throw new UnauthorizedException("User no longer exists.");

            if (!user.IsActive || user.IsDeleted)
                throw new UnauthorizedException("Account is no longer active.");
            // revoke the old refresh token
            existingToken.Revoke();

            var roles = await _userManager.GetRolesAsync(user);

            var newAccessToken =
                await _tokenService.GenerateAccessTokenAsync(user);

            var newRefreshToken =
                _tokenService.GenerateRefreshToken();

            newRefreshToken.UserId = user.Id;

            await _unitOfWork.Repository<RefreshToken>()
                .AddAsync(newRefreshToken, cancellationToken);

            await _unitOfWork.CompleteAsync(cancellationToken);

            return new AuthResponseDto
            {
                AccessToken = newAccessToken.Token,
                AccessTokenExpiresAt = newAccessToken.ExpiresAt,
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiresAt = newRefreshToken.ExpiresAt,
                User = new UserDto
                {
                    Id = user.Id.ToString(),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email!,
                    Roles = roles,
                    ProfilePictureUrl = $"{_configuration["AppSettings:BaseUrl"]}{user.ProfilePictureUrl ?? "default.png"}",
                }
            };
        }

        public async Task<string> ForgotPasswordAsync(string email, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null || !user.EmailConfirmed)
            {
                return "If an account with that email exists, a password reset link has been sent.";
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var resetLink =
                $"{_configuration["AppSettings:BaseUrl"]}" +
                $"/api/auth/reset-password?email={Uri.EscapeDataString(user.Email!)}" +
                $"&token={encodedToken}";

            var htmlBody = $@"but
                <h2>Reset Your Password</h2>
                <p>Hello {user.FirstName},</p>
                <p>We received a request to reset the password for your account.</p>
                <p>To create a new password, click the button below:</p>
                <p>
                    <a href='{resetLink}'>Reset Password</a>
                </p>
                <p>If you did not request a password reset, you can safely ignore this email. Your password will remain unchanged.</p>
                <p>Thank you,<br/>OnlineStore Team</p>";
            await _emailService.SendEmailAsync(user.Email!, "Reset Your Password", htmlBody, cancellationToken);
            return "If an account with that email exists, a password reset link has been sent.";

        }
        public async Task<string> ResetPasswordAsync(string email, string token, string newPassword, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new NotFoundException("User not found.");

            var decodedToken = Encoding.UTF8.GetString(
                WebEncoders.Base64UrlDecode(token));

            var result = await _userManager.ResetPasswordAsync(
                user,
                decodedToken,
                newPassword);

            if (!result.Succeeded)
            {
                throw new BadRequestException(
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            var refreshTokens = await _unitOfWork.Repository<RefreshToken>()
                .GetAllWithSpecAsync(new ActiveRefreshTokensByUserSpecification(user.Id),
                    cancellationToken);

            foreach (var refreshToken in refreshTokens)
            {
                refreshToken.Revoke();
            }

            await _unitOfWork.CompleteAsync(cancellationToken);

            return "Password has been reset successfully.";
        }

        public async Task<UserDto> GetCurrentUserAsync(CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId
                ?? throw new UnauthorizedException("User must be authenticated.");

            var user = await _userManager.FindByIdAsync(userId.ToString())
                ?? throw new NotFoundException("User not found.");

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id.ToString(),
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePictureUrl = $"{_configuration["AppSettings:BaseUrl"]}{user.ProfilePictureUrl ?? "default.png"}",
                Roles = roles.ToList()
            };
        }

        public async Task LogoutAsync(CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId
                ?? throw new UnauthorizedException(
                    "User must be authenticated.");

            var refreshTokens = await _unitOfWork
                .Repository<RefreshToken>()
                .GetAllWithSpecAsync(
                    new ActiveRefreshTokensByUserSpecification(userId));

            foreach (var refreshToken in refreshTokens)
            {
                refreshToken.Revoke();
            }

            await _unitOfWork.CompleteAsync(cancellationToken);
        }




        //public async Task<string> AssignAdminAsync(Guid userId, CancellationToken cancellationToken = default)
        //{
        //    var user = await _userManager.FindByIdAsync(userId.ToString())
        //        ?? throw new NotFoundException("User not found.");

        //    if (await _userManager.IsInRoleAsync(user, Roles.Admin))
        //        throw new BadRequestException("User is already an admin.");

        //    var result = await _userManager.AddToRoleAsync(user, Roles.Admin);
        //    if (!result.Succeeded)
        //        throw new BadRequestException(
        //            string.Join(", ", result.Errors.Select(e => e.Description)));

        //    return "Admin role assigned successfully.";
        //}

        //public async Task<string> UnassignAdminAsync(Guid userId, CancellationToken cancellationToken = default)
        //{
        //    var user = await _userManager.FindByIdAsync(userId.ToString())
        //        ?? throw new NotFoundException("User not found.");

        //    if (!await _userManager.IsInRoleAsync(user, Roles.Admin))
        //        throw new BadRequestException("User is not an admin.");

        //    if (_currentUserService.UserId == userId)
        //        throw new BadRequestException("You cannot remove your own admin role.");

        //    var result = await _userManager.RemoveFromRoleAsync(user, Roles.Admin);
        //    if (!result.Succeeded)
        //        throw new BadRequestException(
        //            string.Join(", ", result.Errors.Select(e => e.Description)));

        //    return "Admin role removed successfully.";
        //}

    }
}


