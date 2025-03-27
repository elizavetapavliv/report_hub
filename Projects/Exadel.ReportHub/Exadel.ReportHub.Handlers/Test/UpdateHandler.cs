using ErrorOr;
using Exadel.ReportHub.Handlers.Common.Errors;
using MediatR;

namespace Exadel.ReportHub.Handlers.Test;

public record UpdateRequest(Guid id, string name, bool getError) : IRequest<ErrorOr<Updated>>;

public class UpdateHandler : IRequestHandler<UpdateRequest, ErrorOr<Updated>>
{
    public Task<ErrorOr<Updated>> Handle(UpdateRequest request, CancellationToken cancellationToken)
    {
        if (request.getError)
        {
            return Task.FromResult<ErrorOr<Updated>>(Errors.Test.ValidationError("Test name is not valid."));
        }

        return Task.FromResult<ErrorOr<Updated>>(Result.Updated);
    }
}
