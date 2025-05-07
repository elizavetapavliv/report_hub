using System.Net.Mail;
using ErrorOr;
using Exadel.ReportHub.Common.Providers;
using Exadel.ReportHub.Email.Abstract;
using Exadel.ReportHub.Email.Models;
using Exadel.ReportHub.RA.Abstract;
using MediatR;

namespace Exadel.ReportHub.Handlers.TestEmail;

public record TestEmailRequest(string Email, string Subject) : IRequest<ErrorOr<TestEmailResult>>;
public class TestEmailHandler(IEmailSender emailSender, IUserRepository userRepository, IUserProvider userProvider) : IRequestHandler<TestEmailRequest, ErrorOr<TestEmailResult>>
{
    public async Task<ErrorOr<TestEmailResult>> Handle(TestEmailRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await userRepository.GetByIdAsync(userProvider.GetUserId(), cancellationToken);
            var reportEmail = new ReportEmailModel
            {
                UserName = user.FullName,
                ReportPeriod = $"{DateTime.Now.Date:dd.MM.yyyy}-{DateTime.Now.Date.AddDays(2):dd.MM.yyyy}",
                IsSuccess = true
            };

            await emailSender.SendAsync(request.Email, request.Subject, "Report.html", reportEmail, cancellationToken);
            return new TestEmailResult { IsSent = true };
        }
        catch
        {
            return new TestEmailResult { IsSent = false };
        }
    }
}
