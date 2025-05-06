using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.User;
using MediatR;

namespace Exadel.ReportHub.Handlers.User.GetByNotification;

public record GetUsersByNotificationRequest : IRequest<ErrorOr<IList<UserDTO>>>;

public class GetUsersByNotificationHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUsersByNotificationRequest, ErrorOr<IList<UserDTO>>>
{
    public async Task<ErrorOr<IList<UserDTO>>> Handle(GetUsersByNotificationRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var currentHour = now.Hour;
        var currentDay = now.Day;
        var currentDayOfWeek = now.DayOfWeek;
        var users = await userRepository.GetUsersByNotificationAsync(currentDay, currentDayOfWeek, currentHour, cancellationToken);

        var result = mapper.Map<List<UserDTO>>(users);
        return result;
    }
}
