using System.Net;

namespace Exadel.ReportHub.Handlers.User.Create.Validators.Exceptions;

public class HttpStatusCodeException : Exception
{
    public IEnumerable<string> Errors { get; }

    public HttpStatusCodeException(IEnumerable<string> errors)
    {
        Errors = errors;
    }
}
