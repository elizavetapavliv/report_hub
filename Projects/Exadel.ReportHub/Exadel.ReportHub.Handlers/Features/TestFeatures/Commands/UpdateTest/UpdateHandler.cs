using ErrorOr;
using Exadel.ReportHub.Handlers.Common.Errors;
using MediatR;

namespace Exadel.ReportHub.Handlers.Features.TestFeatures.Commands.UpdateTest;

public class UpdateHandler : IRequestHandler<UpdateRequest, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(UpdateRequest request, CancellationToken cancellationToken)
    {
        if (request.getError)
        {
            return Errors.Test.ValidationError("Test name is not valid.");
        }

        return Unit.Value;
    }
}
