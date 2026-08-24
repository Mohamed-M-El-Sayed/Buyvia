using MediatR;

namespace OnlineStore.Application.Features.Brands.Commands.DeleteBrand
{
    public class DeleteBrandCommand(int brandId) : IRequest<Unit>
    {
        public int BrandId { get; } = brandId;
    }
}
