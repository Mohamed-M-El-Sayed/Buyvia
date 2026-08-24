using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Orders.Dtos
{
    public class OrderSummaryDto
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public OrderStatus Status { get; set; }

        public decimal Total { get; set; }

        public int TotalItems { get; set; }

        public List<OrderSummaryItemDto> Items { get; set; } = [];
    }
}
