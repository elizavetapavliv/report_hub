using System.Net.Mail;
using ErrorOr;
using Exadel.ReportHub.Email.Abstract;
using MediatR;

namespace Exadel.ReportHub.Handlers.TestEmail;

public record TestEmailRequest(string Email, string Subject, string Body) : IRequest<ErrorOr<TestEmailResult>>;
public class TestEmailHandler(IEmailSender emailSender) : IRequestHandler<TestEmailRequest, ErrorOr<TestEmailResult>>
{
    public async Task<ErrorOr<TestEmailResult>> Handle(TestEmailRequest request, CancellationToken cancellationToken)
    {
        var msg = new MailMessage
        {
            Subject = request.Subject,
            Body = request.Body,
            IsBodyHtml = false
        };
        msg.To.Add(request.Email);
        try
        {
            await emailSender.SendAsync(msg);
            return new TestEmailResult { IsSent = true };
        }
        catch (Exception ex)
        {
            return new TestEmailResult { IsSent = false };
        }
    }
}
