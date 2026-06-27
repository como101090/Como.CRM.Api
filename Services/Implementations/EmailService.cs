using Azure;
using Como.CRM.Api.Data;
using Como.CRM.Api.Options;
using Como.CRM.Api.Services.Abstractions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using MimeKit;

namespace Como.CRM.Api.Services.Implementations;

public class EmailService : IEmailService
{

    private readonly AppDbContext _context;

    public EmailService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> SendAsync(
    string toEmail,
    string subject,
    string body,
    bool isHtml = true,
    CancellationToken cancellationToken = default)
    {
        try
        {
            var settings = await _context.PublisherMailInfos
                .AsNoTracking()
                .FirstAsync(cancellationToken);

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(
                settings.FromName,
                settings.FromEmail));

            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;

            message.Body = new TextPart(isHtml ? "html" : "plain")
            {
                Text = body
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                settings.Host,
                settings.Port,
                SecureSocketOptions.Auto,
                cancellationToken);

            if (!string.IsNullOrWhiteSpace(settings.FromEmail))
            {
               await smtp.AuthenticateAsync(
                    settings.FromEmail,
                    settings.Password,
                    cancellationToken);
            }

            var res = await smtp.SendAsync(message, cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);

            if (res.StartsWith("2"))
                return true;
            else
                return false;
        }
        catch
        {
            return false;
        }
    }


}
