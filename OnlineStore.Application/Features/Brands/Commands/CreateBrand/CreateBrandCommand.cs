using MediatR;

namespace OnlineStore.Application.Features.Brands.Commands.CreateBrand
{
    public class CreateBrandCommand : IRequest<int>
    {
        public string Name { get; set; } = default!;
        public string LogoUrl { get; set; } = default!;

    }
}
