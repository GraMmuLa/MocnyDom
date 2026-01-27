using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace MocnyDom.Application.Email
{
    public class SmtpEmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public SmtpEmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            await SendManyAsync(new[] { to }, subject, body);
        }

        public async Task SendManyAsync(IEnumerable<string> to, string subject, string body)
        {
            using var client = new SmtpClient(_settings.Host, _settings.Port)
            {
                EnableSsl = _settings.EnableSsl,
                Credentials = new NetworkCredential(_settings.Login, _settings.Password)
            };

            using var message = new MailMessage()
            {
                From = new MailAddress(_settings.From),
                Subject = subject,
                Body = body
            };

            foreach (var addr in to)
            {
                message.To.Add(addr);
            }

            await client.SendMailAsync(message);
        }
    }
}
