using System.Diagnostics.CodeAnalysis;
using ErrorOr;
using Exadel.ReportHub.Host.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.ReportHub.Host.Services.Abstract;

[ExcludeFromCodeCoverage]
[ApiController]
[Route("api/[controller]")]
public abstract class BaseService : ControllerBase
{
    protected ActionResult<Created> FromResult(ErrorOr<Created> result)
    {
        return result.Match(
            _ => StatusCode(StatusCodes.Status201Created),
            errors => GetErrorResult<Created>(errors));
    }

    protected ActionResult<Updated> FromResult(ErrorOr<Updated> result)
    {
        return result.Match(
            _ => StatusCode(StatusCodes.Status204NoContent),
            errors => GetErrorResult<Updated>(errors));
    }

    protected ActionResult<Deleted> FromResult(ErrorOr<Deleted> result)
    {
        return result.Match(
            _ => StatusCode(StatusCodes.Status204NoContent),
            errors => GetErrorResult<Deleted>(errors));
    }

    protected ActionResult<TResult> FromResult<TResult>(ErrorOr<TResult> result, int statusCode = StatusCodes.Status200OK)
        where TResult : class
    {
        return result.Match(
            value => StatusCode(statusCode, value),
            errors => GetErrorResult<TResult>(errors));
    }

    private ActionResult<TResult> GetErrorResult<TResult>(List<Error> errors)
    {
        var errorResponse = new ErrorResponse
        {
            Errors = errors.Select(e => e.Description).ToList()
        };
        var statusCode = StatusCodes.Status500InternalServerError;

        if (errors.Any(e => e.Type == ErrorType.Validation))
        {
            statusCode = StatusCodes.Status400BadRequest;
        }
        else if(errors.Any(e => e.Type == ErrorType.Forbidden))
        {
            statusCode = StatusCodes.Status403Forbidden;
        }
        else if (errors.Any(e => e.Type == ErrorType.NotFound))
        {
            statusCode = StatusCodes.Status404NotFound;
        }

        return StatusCode(statusCode, errorResponse);
    }
}
