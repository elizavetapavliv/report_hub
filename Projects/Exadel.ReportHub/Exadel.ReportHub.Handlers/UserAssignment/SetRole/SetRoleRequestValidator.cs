using Exadel.ReportHub.RA.Abstract;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.UserAssignment.SetRole;

public class SetRoleRequestValidator : AbstractValidator<SetRoleRequest>
{
    private readonly IUserRepository _userRepository;
    private readonly IClientRepository _clientRepository;

    public SetRoleRequestValidator(IUserRepository userRepository, IClientRepository clientRepository)
    {
        _userRepository = userRepository;
        _clientRepository = clientRepository;
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleFor(x => x.SetUserAssignmentDTO)
            .ChildRules(child =>
            {
                child.RuleLevelCascadeMode = CascadeMode.Stop;

                child.RuleFor(x => x.UserId)
                    .MustAsync(UserMustExistAsync)
                    .WithMessage(Constants.Validation.UserAssignment.UserNotExistMessage);

                child.RuleFor(x => x.ClientId)
                    .MustAsync(ClientMustExistAsync)
                    .WithMessage(Constants.Validation.UserAssignment.ClientNotExistMessage);

                child.RuleFor(x => x.Role)
                    .IsInEnum();
            });
    }

    private async Task<bool> UserMustExistAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _userRepository.ExistsAsync(userId, cancellationToken);
    }

    private async Task<bool> ClientMustExistAsync(Guid clientId, CancellationToken cancellationToken)
    {
        return clientId.Equals(Constants.Client.GlobalId) || await _clientRepository.ExistsAsync(clientId, cancellationToken);
    }
}
