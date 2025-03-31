using System.Net;

namespace Exadel.ReportHub.Handlers.UserHandlers.Validators.Exceptions;

public class HttpStatusCodeException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public IEnumerable<string> Errors { get; }

    public HttpStatusCodeException(HttpStatusCode httpStatusCode, IEnumerable<string> errors)
        : base(string.Join(", ", errors))
    {
        StatusCode = httpStatusCode;
        Errors = errors;
    }
}
