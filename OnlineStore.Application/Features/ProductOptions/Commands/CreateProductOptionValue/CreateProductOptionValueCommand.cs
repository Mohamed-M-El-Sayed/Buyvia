using System.Text.Json.Serialization;
using MediatR;
using OnlineStore.Application.Features.ProductOptions.Dtos;

namespace OnlineStore.Application.Features.ProductOptions.Commands.CreateProductOptionValue
{
    public class CreateProductOptionValueCommand : IRequest<CreateProductOptionValueResponseDto>
    {
        [JsonIgnore]
        public int OptionId { get; set; }
        public string Value { get; set; } = default!;
    }
}
