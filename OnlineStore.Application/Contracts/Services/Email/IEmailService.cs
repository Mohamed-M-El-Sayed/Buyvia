namespace OnlineStore.Application.Contracts.Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken = default);
    }
}
