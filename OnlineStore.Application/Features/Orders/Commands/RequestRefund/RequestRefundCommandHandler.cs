using MediatR;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Orders.Commands.RequestRefund;
using OnlineStore.Application.Features.Orders.Dtos;
using OnlineStore.Domain.Entities.Orders;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.RefundRequests.Commands.RequestRefund
{
    public class RequestRefundCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUser)
        : IRequestHandler<RequestRefundCommand, Unit>
    {
        public async Task<Unit> Handle(
            RequestRefundCommand request,
            CancellationToken cancellationToken)
        {

            var userId = currentUser.UserId ?? throw new UnauthorizedAccessException(
                "User must be authenticated to request a refund.");

            var specification = new OrderForRefundSpecification(
                request.OrderId,
                userId);

            var order = await unitOfWork.Repository<Order>()
                .GetEntityWithSpecAsync(specification);

            if (order is null)
                throw new NotFoundException(
                    nameof(Order),
                    request.OrderId.ToString());

            if (order.UserId != currentUser.UserId)
                throw new BadRequestException(
                    "You are not allowed to request a refund for this order.");

            if (order.Payment.Status != PaymentStatus.Paid)
                throw new BadRequestException(
                    "Only paid orders can be refunded.");

            var existingRequest = order.RefundRequests
                .FirstOrDefault(x =>
                    x.Status == RefundRequestStatus.Pending);

            if (existingRequest is not null)
                throw new BadRequestException(
                    "A refund request is already pending for this order.");

            var refundRequest = new RefundRequest
            {
                OrderId = order.Id,
                UserId = currentUser.UserId.Value,
                Amount = order.Total,
                Reason = request.Reason,
                Status = RefundRequestStatus.Pending,
                RequestedAt = DateTime.UtcNow
            };

            await unitOfWork.Repository<RefundRequest>()
                .AddAsync(refundRequest);

            await unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}