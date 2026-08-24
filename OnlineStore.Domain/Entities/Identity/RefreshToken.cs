using OnlineStore.Domain.Entities.BaseEntities;

namespace OnlineStore.Domain.Entities.Identity
{
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Token { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; } = false;
        public DateTime? RevokedAt { get; set; }

        public void Revoke()
        {
            IsRevoked = true;
            RevokedAt = DateTime.UtcNow;
        }
        public ApplicationUser User { get; set; } = default!;

    }
}
