using FluentValidation;
using OnlineStore.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(x => x.PurchasedVariantId)
            .GreaterThan(0).WithMessage("PurchasedVariantId is required.");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

        RuleFor(x => x.Title)
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.")
            .When(x => x.Title is not null);

        RuleFor(x => x.Comment)
            .NotEmpty().WithMessage("Comment is required.")
            .MaximumLength(1000).WithMessage("Comment cannot exceed 1000 characters.");
    }
}