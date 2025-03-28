using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using MediatR;

namespace Exadel.ReportHub.Handlers.UserHandlers;

public record UpdateUserActivityRequest(Guid Id, bool IsActive) : IRequest<ErrorOr<Updated>>;

public class UpdateUserActivityHandler(IUserRepository userRepository) : IRequestHandler<UpdateUserActivityRequest, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateUserActivityRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(request.Id, cancellationToken);
        if (user is null)
        {
            return await Task.FromResult<ErrorOr<Updated>>(Error.Validation(description: $"User with '{request.Id} id doesn't exist"));
        }

        await userRepository.UpdateActivityAsync(request.Id, request.IsActive, cancellationToken);
        return await Task.FromResult<ErrorOr<Updated>>(Result.Updated);
    }
}
