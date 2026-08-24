using FluentValidation;

namespace OnlineStore.Application.Features.DeliveryMethods.Commands.UpdateDeliveryMethod;

public class UpdateDeliveryMethodCommandValidator
    : AbstractValidator<UpdateDeliveryMethodCommand>
{
    public UpdateDeliveryMethodCommandValidator()
    {

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Delivery method name is required.")
            .MaximumLength(100)
            .WithMessage("Delivery method name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Delivery method description is required.")
            .MaximumLength(500)
            .WithMessage("Delivery method description cannot exceed 500 characters.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Delivery method price cannot be negative.");

        RuleFor(x => x.EstimatedDeliveryDays)
            .GreaterThan(0)
            .WithMessage("Estimated delivery days must be greater than zero.");
    }
}