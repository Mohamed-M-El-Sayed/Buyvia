using FluentValidation;
using OnlineStore.Domain.Constants;

namespace OnlineStore.Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
    {
        public GetAllUsersQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page number must be at least 1.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(20, 50)
                .WithMessage("Page size must be between 20 and 50.");

            RuleFor(x => x.Role)
                .Must(role => role is null ||
                              role == Roles.Customer ||
                              role == Roles.Admin)
                .WithMessage($"Role must be either '{Roles.Customer}' or '{Roles.Admin}'.");
        }
    }
}