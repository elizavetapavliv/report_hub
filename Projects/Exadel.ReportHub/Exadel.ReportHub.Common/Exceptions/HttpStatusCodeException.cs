﻿using System.Net;

namespace Exadel.ReportHub.Common.Exceptions;

public class HttpStatusCodeException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public IList<string> Errors { get; }

    public HttpStatusCodeException(HttpStatusCode statusCode, IList<string> errors)
        : base(string.Join(',', errors))
    {
        Errors = errors;
        StatusCode = statusCode;
    }
}
