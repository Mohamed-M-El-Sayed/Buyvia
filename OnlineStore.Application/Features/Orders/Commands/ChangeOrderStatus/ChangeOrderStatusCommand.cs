using System.Text.Json.Serialization;
using MediatR;

namespace OnlineStore.Application.Features.Orders.Commands.ChangeOrderStatus
{
    public class ChangeOrderStatusCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int OrderId { get; set; }
        public ChangeOrderStatusOption Status { get; set; }
    }


}
