using System.Text.Json.Serialization;
using MediatR;
using OnlineStore.Application.Features.Discounts.Dtos;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Discounts.Commands.UpdateDiscount
{
    public class UpdateDiscountCommand : IRequest<DiscountDto>
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public DiscountType Type { get; set; }

        public decimal Value { get; set; }

        public decimal? MaxDiscountAmount { get; set; }

        public DateTime? StartsAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
