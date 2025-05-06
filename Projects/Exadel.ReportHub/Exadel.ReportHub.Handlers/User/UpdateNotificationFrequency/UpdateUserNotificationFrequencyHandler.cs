using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.Common.Providers;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.User;
using MediatR;

namespace Exadel.ReportHub.Handlers.User.UpdateNotificationFrequency;

public record UpdateUserNotificationFrequencyRequest(UpdateUserNotificationSettingsDTO UpdateUserNotificationFrequencyDto) : IRequest<ErrorOr<Updated>>;

public class UpdateUserNotificationFrequencyHandler(
    IUserRepository userRepository,
    IMapper mapper,
    IUserProvider userProvider) : IRequestHandler<UpdateUserNotificationFrequencyRequest, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateUserNotificationFrequencyRequest request, CancellationToken cancellationToken)
    {
        var id = userProvider.GetUserId();
        var notificationSettings = mapper.Map<Data.Models.NotificationSettings>(request.UpdateUserNotificationFrequencyDto);
        await userRepository.UpdateNotificationSettingsAsync(id, notificationSettings, cancellationToken);
        return Result.Updated;
    }
}
