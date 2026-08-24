using MediatR;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Discounts.Commands.CreateDiscount
{
    public record CreateDiscountCommand : IRequest<int>
    {
        public string Name { get; set; } = default!;
        public DiscountType Type { get; set; }
        public decimal Value { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public DateTime? StartsAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsEnabled { get; set; } = true;

    }
}
