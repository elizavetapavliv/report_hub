using ErrorOr;
using Exadel.ReportHub.Handlers.Common.Errors;
using MediatR;

namespace Exadel.ReportHub.Handlers.Features.TestFeatures.Commands.AddTest;

public class AddHandler : IRequestHandler<AddRequest, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(AddRequest request, CancellationToken cancellationToken)
    {
        if (request.getError)
        {
            return Errors.Test.AlreadyExist("Already exist error.");
        }

        return Unit.Value;
    }
}
