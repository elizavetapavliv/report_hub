using System.Net;

namespace Exadel.ReportHub.Handlers.User.Create.Exceptions;

public class HttpStatusCodeException : Exception
{
    public IEnumerable<string> Errors { get; }

    public HttpStatusCodeException(IEnumerable<string> errors)
        : base(string.Join(',', errors))
    {
        Errors = errors;
    }
}
