using FluentValidation;
namespace OnlineStore.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(p => p.Name)
             .MaximumLength(150)
             .When(p => p.Name is not null);

            RuleFor(p => p.ShortDescription)
                .MaximumLength(250)
                .When(p => p.ShortDescription is not null);

            RuleFor(p => p.Description)
                .MaximumLength(2000)
                .When(p => p.Description is not null);
        }
    }
}