using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using OnlineStore.API.Extensions;
using OnlineStore.Application.Features.Auth.Commands.ConfirmEmail;
using OnlineStore.Application.Features.Auth.Commands.ForgotPassword;
using OnlineStore.Application.Features.Auth.Commands.Login;
using OnlineStore.Application.Features.Auth.Commands.Logout;
using OnlineStore.Application.Features.Auth.Commands.RefreshToken;
using OnlineStore.Application.Features.Auth.Commands.Register;
using OnlineStore.Application.Features.Auth.Commands.ResendConfirmationEmail;
using OnlineStore.Application.Features.Auth.Commands.ResetPassword;
using OnlineStore.Application.Features.Auth.Commands.UpdateProfile;
using OnlineStore.Application.Features.Auth.Dtos;
using OnlineStore.Application.Features.Auth.Queries.GetCurrentUser;

namespace OnlineStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Registers a new user account.
    /// </summary>
    [HttpPost("register")]
    [EnableRateLimiting(RateLimitingExtensions.AuthStrict)]
    public async Task<ActionResult<RegisterResponseDto>> Register(
        [FromBody] RegisterCommand command,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(command, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Authenticates a user and returns access and refresh tokens.
    /// </summary>
    [HttpPost("login")]
    [EnableRateLimiting(RateLimitingExtensions.AuthStrict)]
    public async Task<ActionResult<AuthResponseDto>> Login(
        [FromBody] LoginCommand command,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(command, cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Confirms a user's email address.
    /// </summary>
    [HttpGet("confirm-email")]
    [EnableRateLimiting(RateLimitingExtensions.AuthModerate)]
    public async Task<IActionResult> ConfirmEmail(
        [FromQuery] ConfirmEmailCommand command,
        CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);

        return Ok(new
        {
            Message = "Email confirmed successfully."
        });
    }

    /// <summary>
    /// Resends the email confirmation link if the account exists and is not yet confirmed.
    /// </summary>
    [HttpPost("resend-confirmation-email")]
    [EnableRateLimiting(RateLimitingExtensions.AuthStrict)]
    public async Task<ActionResult<MessageResponseDto>> ResendConfirmationEmail(
        [FromBody] ResendConfirmationEmailCommand command,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(command, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Gets the currently authenticated user's information.
    /// </summary>
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetCurrentUser(
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(
            new GetCurrentUserQuery(),
            cancellationToken);

        return Ok(response);
    }
    /// <summary>
    /// Updates the profile information of the current user.
    /// </summary>
    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(
        UpdateProfileCommand command,
        CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);

        return NoContent();
    }


    /// <summary>
    /// Generates a new access token using a valid refresh token.
    /// </summary>
    [HttpPost("refresh-token")]
    [EnableRateLimiting(RateLimitingExtensions.AuthModerate)]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken(
        [FromBody] RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(command, cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Logs out the currently authenticated user.
    /// </summary>
    [Authorize]
    [HttpPost("logout")]
    [EnableRateLimiting(RateLimitingExtensions.AuthModerate)]
    public async Task<IActionResult> Logout(
        CancellationToken cancellationToken)
    {
        await mediator.Send(
            new LogoutCommand(),
            cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Sends a password reset email if the account exists.
    /// </summary>
    [HttpPost("forgot-password")]
    [EnableRateLimiting(RateLimitingExtensions.AuthStrict)]
    public async Task<ActionResult<MessageResponseDto>> ForgotPassword(
        [FromBody] ForgotPasswordCommand command,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(command, cancellationToken);

        return Ok(response);
    }

    /// <summary>
    /// Resets the password for a user.
    /// </summary>
    [HttpPost("reset-password")]
    [EnableRateLimiting(RateLimitingExtensions.AuthModerate)]
    public async Task<ActionResult<MessageResponseDto>> ResetPassword(
        [FromBody] ResetPasswordCommand command,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(command, cancellationToken);

        return Ok(response);
    }


}