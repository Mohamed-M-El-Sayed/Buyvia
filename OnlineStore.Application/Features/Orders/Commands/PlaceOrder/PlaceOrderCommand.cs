using MediatR;
using OnlineStore.Application.Features.Orders.Dtos;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Orders.Commands.PlaceOrder
{
    public class PlaceOrderCommand : IRequest<int>
    {
        public int DeliveryMethodId { get; set; }
        public int AddressId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
