using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using MediatR;

namespace Exadel.ReportHub.Handlers.UserHandlers.UpdateActivity;

public record UpdateUserActivityRequest(Guid Id, bool IsActive) : IRequest<ErrorOr<Updated>>;

public class UpdateUserActivityHandler(IUserRepository userRepository) : IRequestHandler<UpdateUserActivityRequest, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateUserActivityRequest request, CancellationToken cancellationToken)
    {
        var isExists = await userRepository.IsExistsAsync(request.Id, cancellationToken);
        if (!isExists)
        {
            return await Task.FromResult<ErrorOr<Updated>>(Error.NotFound());
        }

        await userRepository.UpdateActivityAsync(request.Id, request.IsActive, cancellationToken);
        return await Task.FromResult<ErrorOr<Updated>>(Result.Updated);
    }
}
