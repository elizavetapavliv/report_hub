using ErrorOr;
using Exadel.ReportHub.Host.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.ReportHub.Host.Services;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseService : ControllerBase
{
    protected IActionResult FromResult(ErrorOr<Created> result)
    {
        return result.Match(
            success => Created(),
            errors => GetErrorResult(errors));
    }

    protected IActionResult FromResult(ErrorOr<Updated> result)
    {
        return result.Match(
            success => NoContent(),
            errors => GetErrorResult(errors));
    }

    protected IActionResult FromResult(ErrorOr<Deleted> result)
    {
        return result.Match(
            success => NoContent(),
            errors => GetErrorResult(errors));
    }

    protected IActionResult FromResult<TResult>(ErrorOr<TResult> result, int statusCode = StatusCodes.Status200OK)
        where TResult : class
    {
        return result.Match(
            success => new ObjectResult(success) { StatusCode = statusCode },
            errors => GetErrorResult(errors));
    }

    private IActionResult GetErrorResult(List<Error> errors)
    {
        if (errors.Count == 0)
        {
            return new ObjectResult(new ErrorResponse())
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        var errorResponse = new ErrorResponse
        {
            Errors = errors.Select(e => e.Description).ToList()
        };

        if (errors.Any(e => e.Type == ErrorType.Validation))
        {
            return new ObjectResult(errorResponse)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        }

        if (errors.Any(e => e.Type == ErrorType.Forbidden))
        {
            return new ObjectResult(errorResponse)
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }

        if (errors.Any(e => e.Type == ErrorType.NotFound))
        {
            return new ObjectResult(errorResponse)
            {
                StatusCode = StatusCodes.Status404NotFound
            };
        }

        return new ObjectResult(errorResponse)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}
