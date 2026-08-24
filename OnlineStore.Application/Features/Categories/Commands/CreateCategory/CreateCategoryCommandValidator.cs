using FluentValidation;
using OnlineStore.Application.Contracts.Persistence;

namespace OnlineStore.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(c => c.Name)
                 .NotEmpty().WithMessage("Category name is required")
                 .Length(3, 100).WithMessage("Category name must be between 3 and 100 characters");

            RuleFor(c => c.Description)
                .MaximumLength(500);



        }
    }
}
