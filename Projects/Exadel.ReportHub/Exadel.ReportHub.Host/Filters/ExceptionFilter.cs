using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Exadel.ReportHub.Host.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionFilter(ILogger<ExceptionFilter> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            // 1) Log the exception
            _logger.LogError(context.Exception, "An exception occurred while processing the request");

            // 2) Always return 500 Internal Server Error
            // Show the exception message only in Development mode, hide it in Production
            var errorResponse = new
            {
                Message = "An error occurred while processing your request.",
                Error = _env.IsDevelopment() ? context.Exception.Message : null
            };

            // 3) Set response status and return IActionResult
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}
