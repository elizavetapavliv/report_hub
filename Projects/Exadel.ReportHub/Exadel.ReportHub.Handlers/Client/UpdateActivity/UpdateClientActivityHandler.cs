using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using MediatR;

namespace Exadel.ReportHub.Handlers.Client.UpdateActivity;

public record UpdateClientActivityRequest(Guid Id, bool IsActive) : IRequest<ErrorOr<Updated>>;

public class UpdateClientActivityHandler(IClientRepository clientRepository) : IRequestHandler<UpdateClientActivityRequest, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateClientActivityRequest request, CancellationToken cancellationToken)
    {
        var isExists = await clientRepository.ExistsAsync(request.Id, cancellationToken);
        if (!isExists)
        {
            return Error.NotFound();
        }

        await clientRepository.UpdateActivityAsync(request.Id, request.IsActive, cancellationToken);

        return Result.Updated;
    }
}
