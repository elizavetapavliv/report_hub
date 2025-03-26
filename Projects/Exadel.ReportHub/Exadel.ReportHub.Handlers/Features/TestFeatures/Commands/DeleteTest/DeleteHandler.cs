using ErrorOr;
using Exadel.ReportHub.Handlers.Common.Errors;
using MediatR;

namespace Exadel.ReportHub.Handlers.Features.TestFeatures.Commands.DeleteTest;

public class DeleteHandler : IRequestHandler<DeleteRequest, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(DeleteRequest request, CancellationToken cancellationToken)
    {
        if (request.getError)
        {
            return Errors.Test.DoesNotExist("Does not exist error.");
        }

        return Unit.Value;
    }
}
