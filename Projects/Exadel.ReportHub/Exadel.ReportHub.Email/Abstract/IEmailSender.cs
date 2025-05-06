using System.Net.Mail;

namespace Exadel.ReportHub.Email.Abstract;

public interface IEmailSender
{
    Task SendAsync(MailMessage message);
}
