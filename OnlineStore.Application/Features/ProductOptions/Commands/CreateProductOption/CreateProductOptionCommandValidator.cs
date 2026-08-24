using FluentValidation;

namespace OnlineStore.Application.Features.ProductOptions.Commands.CreateProductOption
{
    public class CreateProductOptionCommandValidator : AbstractValidator<CreateProductOptionCommand>
    {
        public CreateProductOptionCommandValidator()
        {


            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Option name is required")
                .MaximumLength(100).WithMessage("Option name cannot exceed 100 characters");


            RuleFor(x => x.Values)
                .NotNull().WithMessage("Values are required")
                .Must(v => v.Count <= 50).WithMessage("Cannot specify more than 50 option values");

            RuleForEach(x => x.Values).SetValidator(new CreateProductOptionValueValidator());

            RuleFor(x => x.Values)
                .Must(list =>
                {
                    if (list == null) return true;
                    var duplicates = list
                        .Select(v => v.Value?.Trim() ?? string.Empty)
                        .Where(s => !string.IsNullOrEmpty(s))
                        .GroupBy(s => s, StringComparer.OrdinalIgnoreCase)
                        .Any(g => g.Count() > 1);
                    return !duplicates;
                })
                .WithMessage("Option values must be unique (case-insensitive)");
        }
    }

    public class CreateProductOptionValueValidator : AbstractValidator<CreateProductOptionValueRequest>
    {
        public CreateProductOptionValueValidator()
        {
            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("Option value is required")
                .MaximumLength(100).WithMessage("Option value cannot exceed 100 characters");

        }
    }
}
