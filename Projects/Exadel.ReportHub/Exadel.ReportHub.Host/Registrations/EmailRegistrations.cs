using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Mail;
using Exadel.ReportHub.Email;
using Exadel.ReportHub.Email.Abstract;
using Microsoft.Extensions.Options;

namespace Exadel.ReportHub.Host.Registrations;

[ExcludeFromCodeCoverage]
public static class EmailRegistrations
{
    public static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpConfig>(configuration.GetSection(nameof(SmtpConfig)));

        services.AddSingleton(sp =>
        {
            var cfg = sp.GetRequiredService<IOptionsMonitor<SmtpConfig>>().CurrentValue;

            return new SmtpClient(cfg.Host, cfg.Port)
            {
                EnableSsl = cfg.EnableSsl,
                Credentials = new NetworkCredential(cfg.UserName, cfg.Password)
            };
        });
        services.AddSingleton<IEmailSender, EmailSender>();

        return services;
    }
}
