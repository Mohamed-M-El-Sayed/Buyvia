using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using OnlineStore.Application.Contracts.Services.Email;

namespace OnlineStore.Infrastructure.Services.Email
{
    public class EmailService : IEmailService
    {

        private readonly EmailSettings _settings;
        private readonly ILogger<EmailService> _logger;
        public EmailService(IOptions<EmailSettings> options, ILogger<EmailService> logger)
        {
            _settings = options.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken = default)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new BodyBuilder
            {
                HtmlBody = htmlBody
            }.ToMessageBody();
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, cancellationToken);
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password, cancellationToken);
            await smtp.SendAsync(message, cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);

        }
    }
}
