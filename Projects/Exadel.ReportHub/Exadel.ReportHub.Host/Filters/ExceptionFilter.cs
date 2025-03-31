﻿using Exadel.ReportHub.Handlers.User.Create.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Exadel.ReportHub.Host.Filters;

public class ExceptionFilter(ILogger<ExceptionFilter> logger, IHostEnvironment hostEnvironment) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        logger.LogError(context.Exception, "An exception occurred while processing the request");

        if(context.Exception is HttpStatusCodeException httpStatusCodeException)
        {
            context.Result = CreateStatusCodeErrorResult(httpStatusCodeException);
        }
        else
        {
            context.Result = CreateErrorResult(context.Exception, hostEnvironment);
        }

        context.ExceptionHandled = true;
    }

    private IActionResult CreateStatusCodeErrorResult(HttpStatusCodeException exception)
    {
        return new BadRequestObjectResult(new ErrorResponce { Errors = exception.Message });
    }

    private IActionResult CreateErrorResult(Exception exception, IHostEnvironment hostEnvironment)
    {
        if (hostEnvironment.IsDevelopment())
        {
            return new ObjectResult(new ErrorResponce { Errors = exception.Message });
        }

        return new ObjectResult(new ErrorResponce { Errors = "An unexpected error occured" })
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }

    private sealed class ErrorResponce
    {
        public string Errors { get; set; }
    }
}
