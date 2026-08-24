using FluentValidation;
using OnlineStore.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(150);

        RuleFor(p => p.ShortDescription)
            .NotEmpty().WithMessage("Short description is required.")
            .MaximumLength(250);

        RuleFor(p => p.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(2000);

        RuleFor(p => p.CategoryId)
            .NotEmpty().WithMessage("Category is required.")
            .GreaterThan(0).WithMessage("Valid category is required.");

        RuleFor(p => p.BrandId)
            .NotEmpty().WithMessage("Brand is required.")
            .GreaterThan(0).WithMessage("Valid brand is required.");

        //RuleFor(p => p.IsActive)
        //    .NotNull();

        //RuleFor(p => p.InitialVariant)
        //    .NotNull().WithMessage("Initial variant is required.")
        //    .SetValidator(new CreateInitialVariantValidator());
    }

}
//public class CreateInitialVariantValidator : AbstractValidator<CreateInitialVariantDto>
//{
//    public CreateInitialVariantValidator()
//    {
//        RuleFor(x => x.Price)
//            .GreaterThan(0).WithMessage("Price must be greater than 0.");

//        RuleFor(x => x.Stock)
//            .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");

//        RuleFor(x => x.StockThreshold)
//            .GreaterThanOrEqualTo(0)
//            .WithMessage("Stock threshold cannot be negative.")
//            .LessThanOrEqualTo(x => x.Stock)
//            .WithMessage("Stock threshold cannot be greater than stock.");

//        RuleFor(x => x.MainImageUrl)
//            .NotEmpty().WithMessage("Main image is required.")
//            .MaximumLength(500);

//        RuleFor(x => x.AdditionalImagesUrls)
//            .Must(list => list.Count <= 10)
//            .WithMessage("You cannot upload more than 10 images.");

//        RuleForEach(x => x.AdditionalImagesUrls)
//            .NotEmpty()
//            .MaximumLength(500);

//        RuleFor(x => x.AttributeValueIds)
//            .Must(list => list.Distinct().Count() == list.Count)
//            .WithMessage("Duplicate attribute values are not allowed.");
//    }
//}