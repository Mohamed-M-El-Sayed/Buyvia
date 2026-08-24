using FluentValidation;
using OnlineStore.Application.Contracts.Persistence;

namespace OnlineStore.Application.Features.Brands.Commands.UpdateBrand
{
    public class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
    {
        public UpdateBrandCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Brand id must be greater than 0");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Brand name is required")
                .MaximumLength(100).WithMessage("Brand name cannot exceed 100 characters");
            RuleFor(b => b.LogoUrl)
                        .Matches(@"^/Brand/[^/\\]+\.(jpg|jpeg|png|webp)$")
                        .WithMessage("Logo URL must be in the format /Brand/filename with a valid image extension.");
        }
    }
}
