using ErrorOr;
using Exadel.ReportHub.Common.Providers;
using Exadel.ReportHub.RA.Abstract;
using MediatR;

namespace Exadel.ReportHub.Handlers.User.UpdateName;

public record UpdateUserNameRequest(string FullName) : IRequest<ErrorOr<Updated>>;

public class UpdateUserNameHandler(IUserRepository userRepository, IUserProvider userProvider) : IRequestHandler<UpdateUserNameRequest, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateUserNameRequest request, CancellationToken cancellationToken)
    {
        var userId = userProvider.GetUserId();
        await userRepository.UpdateNameAsync(userId, request.FullName, cancellationToken);

        return Result.Updated;
    }
}
