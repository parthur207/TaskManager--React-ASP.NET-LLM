using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using TaskManager.Core.Ports.Emails;

namespace TaskManager.Adapters.ExternalServices.Email
{
    public class EmailSenderAdapter : IEmailSenderPort
    {
        private readonly SmtpSettings _settings;

        public EmailSenderAdapter(IOptions<SmtpSettings> settings)
        {
            _settings = settings.Value;
        }

        public Task Send2FAEmailAsync(string email, string code)
            => SendAsync(email, "Seu código de verificação",
                $"<p>Seu código de verificação é: <strong>{code}</strong></p><p>Expira em 5 minutos.</p>");

        public Task SendAccountConfirmationAsync(string email)
            => SendAsync(email, "Conta confirmada", "<p>Sua conta foi confirmada com sucesso.</p>");

        public Task SendPasswordResetAsync(string email, string resetLink)
            => SendAsync(email, "Redefinição de senha",
                $"<p>Para redefinir sua senha, acesse: <a href=\"{resetLink}\">{resetLink}</a></p>");

        private async Task SendAsync(string toEmail, string subject, string htmlBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new BodyBuilder { HtmlBody = htmlBody }.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);

            if (!string.IsNullOrWhiteSpace(_settings.Username))
                await smtp.AuthenticateAsync(_settings.Username, _settings.Password);

            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(quit: true);
        }
    }
}