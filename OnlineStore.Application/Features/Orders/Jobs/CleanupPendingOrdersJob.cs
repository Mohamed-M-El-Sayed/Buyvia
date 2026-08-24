using Microsoft.Extensions.Logging;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Contracts.Services.BackgroundJobs;
using OnlineStore.Application.Features.Orders.Specifications;
using OnlineStore.Domain.Entities.Orders;
using OnlineStore.Domain.Entities.Products;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Orders.Jobs
{
    public class CleanupPendingOrdersJob(
    IUnitOfWork unitOfWork,
    ILogger<CleanupPendingOrdersJob> logger)
    : ICleanupPendingOrdersJob
    {
        public async Task ExecuteAsync()
        {
            logger.LogInformation(
                "Starting cleanup of expired pending orders.");

            var specification = new PendingExpiredOrdersSpecification();

            var orders = await unitOfWork
                .Repository<Order>()
                .GetAllWithSpecAsync(specification);

            foreach (var order in orders)
            {
                if (order.Status != OrderStatus.Pending)
                    continue;

                foreach (var item in order.Items)
                {
                    var variant = item.ProductVariant;

                    variant.Stock += item.Quantity;

                    unitOfWork
                        .Repository<ProductVariant>()
                        .Update(variant);
                }

                order.Status = OrderStatus.Expired;
                order.Payment.Status = PaymentStatus.Cancelled;

                unitOfWork
                    .Repository<Order>()
                    .Update(order);

                logger.LogInformation(
                    "Order {OrderId} expired. Stock restored and payment cancelled.",
                    order.Id);
            }

            await unitOfWork.CompleteAsync();

            logger.LogInformation(
                "Expired pending orders cleanup completed. Processed {Count} orders.",
                orders.Count());
        }
    }
}