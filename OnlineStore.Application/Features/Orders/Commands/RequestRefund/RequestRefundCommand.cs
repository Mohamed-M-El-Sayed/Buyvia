using System.Text.Json.Serialization;
using MediatR;

namespace OnlineStore.Application.Features.Orders.Commands.RequestRefund
{
    public class RequestRefundCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int OrderId { get; set; }
        public string Reason { get; set; } = default!;
    }
}
