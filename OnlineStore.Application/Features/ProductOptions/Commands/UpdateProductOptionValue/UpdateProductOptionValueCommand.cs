using System.Text.Json.Serialization;
using MediatR;

namespace OnlineStore.Application.Features.ProductOptions.Commands.UpdateProductOptionValue
{
    public class UpdateProductOptionValueCommand : IRequest
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Value { get; set; } = default!;
    }
}
