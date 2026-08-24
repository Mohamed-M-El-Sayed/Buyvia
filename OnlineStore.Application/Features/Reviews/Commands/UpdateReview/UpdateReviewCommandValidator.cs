using FluentValidation;

namespace OnlineStore.Application.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
    {
        public UpdateReviewCommandValidator()
        {
            RuleFor(x => x.ReviewId)
                .GreaterThan(0).WithMessage("ReviewId is required.");

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
}
