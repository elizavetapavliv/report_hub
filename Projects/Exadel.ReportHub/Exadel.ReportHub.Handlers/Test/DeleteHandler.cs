using ErrorOr;
using Exadel.ReportHub.Handlers.Common.Errors;
using MediatR;

namespace Exadel.ReportHub.Handlers.Test;

public record DeleteRequest(Guid id, bool getError) : IRequest<ErrorOr<Deleted>>;

public class DeleteHandler : IRequestHandler<DeleteRequest, ErrorOr<Deleted>>
{
    public Task<ErrorOr<Deleted>> Handle(DeleteRequest request, CancellationToken cancellationToken)
    {
        if (request.getError)
        {
            return Task.FromResult<ErrorOr<Deleted>>(Errors.Test.DoesNotExist("Does not exist error."));
        }

        return Task.FromResult<ErrorOr<Deleted>>(Result.Deleted);
    }
}
