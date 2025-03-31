﻿using Exadel.ReportHub.Handlers.UserHandlers.Validators.Exceptions;
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
            context.Result = CreateErrorResult(httpStatusCodeException, hostEnvironment);
            context.ExceptionHandled = true;
        }
        else
        {
            context.Result = CreateErrorResult(context.Exception, hostEnvironment);
            context.ExceptionHandled = true;
        }
    }

    private IActionResult CreateErrorResult(Exception exception, IHostEnvironment hostEnvironment)
    {
        if (hostEnvironment.IsDevelopment())
        {
            return new ObjectResult(new { Message = exception.Message });
        }

        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}
