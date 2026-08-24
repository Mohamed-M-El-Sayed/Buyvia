using FluentValidation;
using OnlineStore.Application.Contracts.Persistence;

namespace OnlineStore.Application.Features.Brands.Commands.CreateBrand
{
    public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
    {

        public CreateBrandCommandValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(b => b.Name)
                .MaximumLength(100).WithMessage("Brand Name cannot exceed 100 characters");


            RuleFor(b => b.LogoUrl)
                .Matches(@"^/Brand/[^/\\]+\.(jpg|jpeg|png|webp)$")
                .WithMessage("Logo URL must be in the format /Brand/filename with a valid image extension.");
        }
    }
}