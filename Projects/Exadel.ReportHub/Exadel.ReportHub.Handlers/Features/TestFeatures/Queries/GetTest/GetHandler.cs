using ErrorOr;
using Exadel.ReportHub.Handlers.Common.Errors;
using MediatR;

namespace Exadel.ReportHub.Handlers.Features.TestFeatures.Queries.GetTest;

public class GetHandler : IRequestHandler<GetRequest, ErrorOr<string>>
{
    public async Task<ErrorOr<string>> Handle(GetRequest request, CancellationToken cancellationToken)
    {
        if (request.getError)
        {
            return Errors.Test.DoesNotExist("Does not exist error.");
        }

        return "Get test success.";
    }
}
