using System.Text.Json.Serialization;
using MediatR;

namespace OnlineStore.Application.Features.ProductOptions.Commands.UpdateProductOption
{
    public class UpdateProductOptionCommand : IRequest
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; } = default!;
    }
}
