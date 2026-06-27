namespace Como.CRM.Api.Services.Abstractions;

public interface IEmailService
{
    Task<bool> SendAsync(
         string toEmail,
         string subject,
         string body,
         bool isHtml = true,
         CancellationToken cancellationToken = default);
}
