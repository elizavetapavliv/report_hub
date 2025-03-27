using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.ReportHub.Host.Services;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseService : ControllerBase
{
    protected readonly ISender _sender;

    protected BaseService(ISender sender)
    {
        _sender = sender;
    }

    protected IActionResult FromResult(ErrorOr<Created> result)
    {
        return result.Match(
            success => Created(),
            errors => Problem(errors));
    }

    protected IActionResult FromResult(ErrorOr<Updated> result)
    {
        return result.Match(
            success => NoContent(),
            errors => Problem(errors));
    }

    protected IActionResult FromResult(ErrorOr<Deleted> result)
    {
        return result.Match(
            success => NoContent(),
            errors => Problem(errors));
    }

    protected IActionResult FromResult<TResult>(ErrorOr<TResult> result)
        where TResult : class
    {
        return result.Match(
            success => Ok(success),
            errors => Problem(errors));
    }

    private IActionResult Problem(List<Error> errors)
    {
        if (errors.Count == 0)
        {
            return Problem();
        }

        return Problem(errors[0]);
    }

    private IActionResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, title: error.Description);
    }
}
