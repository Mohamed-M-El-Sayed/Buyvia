using Microsoft.Extensions.Logging;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Contracts.Services.Authentication;
using OnlineStore.Application.Contracts.Services.BackgroundJobs;
using OnlineStore.Application.Contracts.Services.Email;
using OnlineStore.Application.Features.ProductVariants.Specifications;
using OnlineStore.Domain.Constants;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Application.Features.ProductVariants.Jobs
{
    public class LowStockCheckJob(IUnitOfWork unitOfWork,
        ILogger<LowStockCheckJob> logger,
        IEmailService emailService,
        IUserService userService) : ILowStockCheckJob
    {
        public async Task ExecuteAsync(IEnumerable<int> productVariantIds)
        {
            var variantIds = productVariantIds.Distinct().ToList();
            if (!variantIds.Any())
                return;

            var variants = await unitOfWork.Repository<ProductVariant>()
                .GetAllWithSpecAsync(new LowStockVariantsSpecification(variantIds));

            if (!variants.Any())
                return;

            var admins = await userService.GetUsersInRoleAsync(Roles.Admin);
            var adminEmails = admins.Select(a => a.Email).Where(e => !string.IsNullOrEmpty(e)).ToList();

            if (!adminEmails.Any())
            {
                logger.LogWarning("Low stock detected but no admin recipients found.");
                return;
            }

            var body = BuildLowStockEmail(variants);
            foreach (var email in adminEmails)
                await emailService.SendEmailAsync(email!, "Low Stock Alert", body);

            foreach (var v in variants)
                v.LowStockAlertedAt = DateTime.UtcNow;

            await unitOfWork.CompleteAsync();
        }
        private static string BuildLowStockEmail(IEnumerable<ProductVariant> variants)
        {
            var items = string.Join(
                "",
                variants.Select(v =>
                    $"""
                    <tr>
                        <td>{v.Id}</td>
                        <td>{v.Stock}</td>
                    </tr>
                    """));

            return $"""
                <!DOCTYPE html>
                <html>
                <body>
                    <h2>Low Stock Alert</h2>
                    <p>
                        The following product variants have reached
                        the low-stock threshold:
                    </p>

                    <table border="1" cellpadding="8">
                        <thead>
                            <tr>
                                <th>Variant ID</th>
                                <th>Current Stock</th>
                            </tr>
                        </thead>

                        <tbody>
                            {items}
                        </tbody>
                    </table>

                    <p>
                        Please review the inventory and restock when necessary.
                    </p>
                    <p>
                        <strong>Online Store</strong>
                    </p>
                </body>
                </html>
                """;
        }
    }
}
