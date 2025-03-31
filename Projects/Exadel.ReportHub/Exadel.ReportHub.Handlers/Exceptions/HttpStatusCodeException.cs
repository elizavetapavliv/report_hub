using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Exadel.ReportHub.Handlers.Exceptions;

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
