using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Exadel.ReportHub.Host.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var statusCode = context.Exception switch
            {
                ArgumentException => HttpStatusCode.BadRequest,      // 400 Bad Request
                UnauthorizedAccessException => HttpStatusCode.Unauthorized, // 401 Unauthorized
                InvalidOperationException => HttpStatusCode.NotFound, // 404 Not Found
                _ => HttpStatusCode.InternalServerError // 500 Internal Server Error
            };

            var errorResponse = new
            {
                Message = "An error occurred while processing your request.",
                Error = context.Exception.Message,
                StatusCode = (int)statusCode
            };

            context.HttpContext.Response.StatusCode = (int)statusCode;
            context.Result = new ObjectResult(errorResponse);
            context.ExceptionHandled = true;
        }
    }
}
