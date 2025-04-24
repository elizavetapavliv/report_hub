using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.Enums;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.UserAssignment.Upsert;

public class UpsertUserAssignmentRequestValidator : AbstractValidator<UpsertUserAssignmentRequest>
{
    private readonly IUserRepository _userRepository;
    private readonly IClientRepository _clientRepository;

    public UpsertUserAssignmentRequestValidator(IUserRepository userRepository, IClientRepository clientRepository)
    {
        _userRepository = userRepository;
        _clientRepository = clientRepository;
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleFor(x => x.UpsertUserAssignmentDto)
            .ChildRules(child =>
            {
                child.RuleLevelCascadeMode = CascadeMode.Stop;

                child.RuleFor(x => x.UserId)
                    .NotEmpty()
                    .MustAsync(_userRepository.ExistsAsync)
                    .WithMessage(Constants.Validation.User.DoesNotExist);

                child.RuleFor(x => x.ClientId)
                    .NotEmpty()
                    .MustAsync(_clientRepository.ExistsAsync)
                    .WithMessage(Constants.Validation.Client.DoesNotExist);

                child.RuleFor(x => x.Role)
                    .IsInEnum();
            });

        RuleFor(x => x.UpsertUserAssignmentDto)
            .Must(dto => !(dto.Role == UserRole.SuperAdmin && dto.ClientId != Constants.Client.GlobalId))
            .WithMessage(Constants.Validation.UserAssignment.GlobalRoleAssignment)
            .Must(dto => !(dto.Role != UserRole.SuperAdmin && dto.ClientId == Constants.Client.GlobalId))
            .WithMessage(Constants.Validation.UserAssignment.ClientRoleAssignment)
            .When(x => x.UpsertUserAssignmentDto.ClientId != Guid.Empty)
            .When(x => Enum.IsDefined(typeof(UserRole), x.UpsertUserAssignmentDto.Role));
    }
}
