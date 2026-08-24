using FluentValidation;

namespace OnlineStore.Application.Features.VariantImages.Command.AddVariantImages
{
    public class AddVariantImagesCommandValidator : AbstractValidator<AddVariantImagesCommand>
    {

        public AddVariantImagesCommandValidator()
        {
            RuleFor(x => x.VariantId)
                .GreaterThan(0).WithMessage("Valid variant ID is required.");

            RuleFor(x => x.Images)
                .NotEmpty().WithMessage("At least one image is required.");
            RuleFor(x => x.Images)
                .Must(imges => imges.Count(i => i.IsMainImage) <= 1)
                .WithMessage("Only one main image is allowed.");

            RuleForEach(x => x.Images)
               .ChildRules(image =>
               {
                   image.RuleFor(i => i.ImageUrl)
                       .NotEmpty()
                       .MaximumLength(500);

                   image.RuleFor(i => i.ImageUrl)
                          .Must(url => Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
                          .WithMessage("Image URL must be a valid URL.");
               });

        }
    }
}
