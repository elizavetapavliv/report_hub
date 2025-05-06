using System.Net.Mail;
using Exadel.ReportHub.Email.Abstract;
using Microsoft.Extensions.Options;

namespace Exadel.ReportHub.Email;

public class EmailSender(SmtpClient smtpClient, IOptionsMonitor<SmtpConfig> smtpConfig) : IEmailSender
{
    public Task SendAsync(MailMessage message)
    {
        if(message.From is null)
        {
            message.From = new MailAddress(smtpConfig.CurrentValue.UserName);
        }

        return smtpClient.SendMailAsync(message);
    }
}
