using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.User;
using MediatR;

namespace Exadel.ReportHub.Handlers.User.UpdateNotificationFrequency;

public record UpdateUserNotificationFrequencyRequest(Guid Id, UpdateUserNotificationFrequencyDTO UpdateUserNotificationFrequencyDto) : IRequest<ErrorOr<Updated>>;

public class UpdateUserNotificationFrequencyHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<UpdateUserNotificationFrequencyRequest, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateUserNotificationFrequencyRequest request, CancellationToken cancellationToken)
    {
        var isExists = await userRepository.ExistsAsync(request.Id, cancellationToken);
        if (!isExists)
        {
            return Error.NotFound();
        }

        var datamap = mapper.Map<Data.Models.User>(request.UpdateUserNotificationFrequencyDto);
        datamap.Id = request.Id;
        await userRepository.UpdateNotificationFrequencyAsync(datamap, cancellationToken);
        return Result.Updated;
    }
}
