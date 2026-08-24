using AutoMapper;
using MediatR;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Discounts.Dtos;
using OnlineStore.Domain.Entities.Promotions;

namespace OnlineStore.Application.Features.Discounts.Queries.GetDiscountById
{
    public class GetDiscountByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetDiscountByIdQuery, DiscountDto>
    {
        public async Task<DiscountDto> Handle(GetDiscountByIdQuery request, CancellationToken cancellationToken)
        {
            var discount = await unitOfWork.Repository<Discount>().GetByIdAsync(request.DiscountId)
                ?? throw new NotFoundException(nameof(Discount), request.DiscountId.ToString());

            var dto = mapper.Map<DiscountDto>(discount);
            return dto;
        }
    }
}
