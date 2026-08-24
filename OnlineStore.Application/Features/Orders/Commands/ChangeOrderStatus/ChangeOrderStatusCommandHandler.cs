using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Orders;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Orders.Commands.ChangeOrderStatus
{
    public class ChangeOrderStatusCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<ChangeOrderStatusCommandHandler> logger)
        : IRequestHandler<ChangeOrderStatusCommand, Unit>
    {
        public async Task<Unit> Handle(
            ChangeOrderStatusCommand request,
            CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Repository<Order>()
                .GetByIdAsync(request.OrderId)
                ?? throw new NotFoundException(
                    nameof(Order),
                    request.OrderId.ToString());

            var newStatus = request.Status switch
            {
                ChangeOrderStatusOption.Processing => OrderStatus.Processing,
                ChangeOrderStatusOption.Shipped => OrderStatus.Shipped,
                ChangeOrderStatusOption.Delivered => OrderStatus.Delivered,
                _ => throw new BadRequestException("Invalid order status.")
            };

            if (!IsValidTransition(order.Status, newStatus))
            {
                throw new BadRequestException(
                    $"Cannot change order status from '{order.Status}' to '{newStatus}'.");
            }

            logger.LogInformation(
                "Changing order {OrderId} status from {OldStatus} to {NewStatus}.",
                order.Id,
                order.Status,
                newStatus);

            order.Status = newStatus;

            unitOfWork.Repository<Order>().Update(order);

            await unitOfWork.CompleteAsync(cancellationToken);
            return Unit.Value;
        }

        private static bool IsValidTransition(
       OrderStatus currentStatus,
       OrderStatus newStatus)
        {
            return currentStatus switch
            {
                OrderStatus.Pending =>
                    newStatus == OrderStatus.Processing,

                OrderStatus.PaymentReceived =>
                    newStatus == OrderStatus.Processing,

                OrderStatus.Processing =>
                    newStatus == OrderStatus.Shipped,

                OrderStatus.Shipped =>
                    newStatus == OrderStatus.Delivered,

                OrderStatus.Delivered => false,

                OrderStatus.Cancelled => false,

                OrderStatus.Refunded => false,

                OrderStatus.Expired => false,

                _ => false
            };
        }
    }
}