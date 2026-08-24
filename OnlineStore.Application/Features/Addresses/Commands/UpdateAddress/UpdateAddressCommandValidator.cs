using FluentValidation;

namespace OnlineStore.Application.Features.Addresses.Commands.UpdateAddress
{
    public class UpdateAddressCommandValidator
        : AbstractValidator<UpdateAddressCommand>
    {
        public UpdateAddressCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(100)
                .Matches(@"^[A-Za-z\s'-]+$")
                .WithMessage("First name contains invalid characters.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(100)
                .Matches(@"^[A-Za-z\s'-]+$")
                .WithMessage("Last name contains invalid characters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(@"^\+201[0125][0-9]{8}$")
                .WithMessage("Phone number must be a valid Egyptian mobile number.");

            RuleFor(x => x.Street)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(250);

            RuleFor(x => x.City)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(100);

            RuleFor(x => x.Country)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(100);
        }
    }
}