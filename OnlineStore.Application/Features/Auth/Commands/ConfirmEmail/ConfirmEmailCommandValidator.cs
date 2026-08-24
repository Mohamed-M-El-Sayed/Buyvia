using FluentValidation;

namespace OnlineStore.Application.Features.Auth.Commands.ConfirmEmail;

public sealed class ConfirmEmailCommandValidator
    : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User id is required.");

        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Confirmation token is required.");
    }
}