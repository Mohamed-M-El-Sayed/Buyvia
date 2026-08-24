using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.Common.Models;
using OnlineStore.Application.Features.Orders.Commands.ApproveRefund;
using OnlineStore.Application.Features.Orders.Commands.ChangeOrderStatus;
using OnlineStore.Application.Features.Orders.Commands.PlaceOrder;
using OnlineStore.Application.Features.Orders.Commands.RejectRefund;
using OnlineStore.Application.Features.Orders.Commands.RequestRefund;
using OnlineStore.Application.Features.Orders.Dtos;
using OnlineStore.Application.Features.Orders.Queries.GetMyOrders;
using OnlineStore.Application.Features.Orders.Queries.GetMyRefundRequests;
using OnlineStore.Application.Features.Orders.Queries.GetOrderById;
using OnlineStore.Application.Features.Orders.Queries.GetRefundRequests;
using OnlineStore.Domain.Constants;

namespace OnlineStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Places a new order for the authenticated user.
        /// </summary>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> PlaceOrder(
            PlaceOrderCommand command,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetOrderById),
                new { orderId = result },
                null);
        }

        /// <summary>
        /// Gets the current user's orders.
        /// </summary>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PageResult<OrderSummaryDto>>> GetMyOrders(
            [FromQuery] GetMyOrdersQuery query,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Gets the details of a specific order.
        /// </summary>
        [Authorize]
        [HttpGet("{orderId:int}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(
            [FromRoute] int orderId,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new GetOrderByIdQuery(orderId),
                cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Changes the status of an order. (Admin only)
        /// </summary>
        [HttpPatch("/api/orders/{orderId:int}/status")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> ChangeOrderStatus(
            [FromRoute] int orderId,
            [FromBody] ChangeOrderStatusCommand command,
            CancellationToken cancellationToken)
        {
            command.OrderId = orderId;
            await mediator.Send(command, cancellationToken);
            return NoContent();
        }

        /// <summary>
        /// Requests a refund for an eligible order.
        /// </summary>
        [Authorize]
        [HttpPost("{orderId:int}/refund-request")]
        public async Task<IActionResult> RequestRefund(
            [FromRoute] int orderId,
            [FromBody] RequestRefundCommand command,
            CancellationToken cancellationToken)
        {
            command.OrderId = orderId;

            await mediator.Send(
                command,
                cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Approves a refund request (Admin only).
        /// </summary>
        [Authorize(Roles = Roles.Admin)]
        [HttpPost("{refundRequestId:int}/approve")]
        public async Task<IActionResult> ApproveRefund(
            [FromRoute] int refundRequestId,
            CancellationToken cancellationToken)
        {
            await mediator.Send(
                new ApproveRefundCommand(refundRequestId),
                cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Rejects a refund request (Admin only).
        /// </summary>
        [Authorize(Roles = Roles.Admin)]
        [HttpPost("{refundRequestId:int}/reject")]
        public async Task<IActionResult> Reject(
            [FromRoute] int refundRequestId,
            [FromBody] RejectRefundCommand command,
            CancellationToken cancellationToken)
        {
            command.RefundRequestId = refundRequestId;

            await mediator.Send(
                command,
                cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Gets the current user's refund requests.
        /// </summary>
        [Authorize]
        [HttpGet("my-refund-requests")]
        public async Task<ActionResult<PageResult<RefundRequestDto>>> GetMyRefundRequests(
            [FromQuery] GetMyRefundRequestsQuery query,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                query,
                cancellationToken);

            return Ok(result);
        }


        /// <summary>
        /// Gets all refund requests (Admin only).
        /// </summary>
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("refund-requests")]
        public async Task<ActionResult<PageResult<RefundRequestDto>>> GetRefundRequests(
            [FromQuery] GetRefundRequestsQuery query,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                query,
                cancellationToken);

            return Ok(result);
        }

    }
}