using System.Text.Json.Serialization;
using MediatR;

namespace OnlineStore.Application.Features.Brands.Commands.UpdateBrand
{
    public class UpdateBrandCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string LogoUrl { get; set; } = default!;

    }
}
