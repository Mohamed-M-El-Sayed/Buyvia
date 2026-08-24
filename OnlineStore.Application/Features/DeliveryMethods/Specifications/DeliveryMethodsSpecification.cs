using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Application.Features.DeliveryMethods.Specifications;

public class DeliveryMethodsSpecification : BaseSpecification<DeliveryMethod>
{
    public DeliveryMethodsSpecification(
        string? search,
        bool? isActive,
        int pageIndex = 1,
        int pageSize = 10,
        bool enablePagination = true)
    {
        Criteria = x =>
            (!isActive.HasValue || x.IsActive == isActive.Value) &&
            (
                string.IsNullOrWhiteSpace(search) ||
                x.Name.Contains(search) ||
                (x.Description != null &&
                 x.Description.Contains(search))
            );

        if (enablePagination)
        {
            ApplyPagination(pageSize, pageIndex);
        }
        AsNoTracking();
    }
}