using System.Text.Json.Serialization;
using MediatR;
using OnlineStore.Application.Features.ProductOptions.Dtos;

namespace OnlineStore.Application.Features.ProductOptions.Commands.CreateProductOption
{
    public class CreateProductOptionCommand : IRequest<CreateProductOptionResponseDto>
    {
        [JsonIgnore]
        public int ProductId { get; set; }
        public string Name { get; set; } = default!;
        public List<CreateProductOptionValueRequest> Values { get; set; } = new();
    }

    public class CreateProductOptionValueRequest
    {
        public string Value { get; set; } = default!;
    }
}
