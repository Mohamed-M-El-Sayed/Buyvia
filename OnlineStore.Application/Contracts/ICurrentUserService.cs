namespace OnlineStore.Application.Contracts
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        //string FullName { get; }
        //Task<string> GetFullNameAsync(Guid userId, CancellationToken cancellationToken = default);
        bool IsAdmin { get; }

    }
}
