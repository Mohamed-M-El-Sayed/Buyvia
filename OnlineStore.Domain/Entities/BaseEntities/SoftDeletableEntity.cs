namespace OnlineStore.Domain.Entities.BaseEntities
{
    public abstract class SoftDeletableEntity : BaseEntity
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual void Delete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }
        public virtual void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
        }
    }
}
