using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Identity;

namespace OnlineStore.Application.Features.Auth.Specifications
{
    public class ActiveRefreshTokensByUserSpecification : BaseSpecification<RefreshToken>
    {
        public ActiveRefreshTokensByUserSpecification(Guid userId)
        {
            Criteria = rt => rt.UserId == userId && !rt.IsRevoked;
        }
    }
}
