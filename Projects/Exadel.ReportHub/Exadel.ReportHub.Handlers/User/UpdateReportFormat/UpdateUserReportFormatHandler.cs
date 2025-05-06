using ErrorOr;
using Exadel.ReportHub.Data.Enums;
using Exadel.ReportHub.RA.Abstract;
using MediatR;

namespace Exadel.ReportHub.Handlers.User.UpdateReportFormat;

public record UpdateUserReportRequest(Guid Id, ReportFormat ReportFormat) : IRequest<ErrorOr<Updated>>;

public class UpdateUserReportFormatHandler(IUserRepository userRepository) : IRequestHandler<UpdateUserReportRequest, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateUserReportRequest request, CancellationToken cancellationToken)
    {
        var isExists = await userRepository.ExistsAsync(request.Id, cancellationToken);
        if (!isExists)
        {
            return Error.NotFound();
        }

        await userRepository.UpdateReportFormatAsync(request.Id, request.ReportFormat, cancellationToken);
        return Result.Updated;
    }
}
