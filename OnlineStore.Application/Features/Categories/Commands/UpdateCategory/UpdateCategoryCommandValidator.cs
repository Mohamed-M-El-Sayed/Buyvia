using FluentValidation;

namespace OnlineStore.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("CategoryId is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                 .Length(3, 100).WithMessage("Category name must be between 3 and 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }
    }
}
