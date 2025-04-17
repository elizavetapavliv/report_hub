using Exadel.ReportHub.RA.Abstract;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.User.Create;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<string> _validator;

    public CreateUserRequestValidator(IUserRepository userRepository, IValidator<string> validator)
    {
        _userRepository = userRepository;
        _validator = validator;
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleFor(x => x.CreateUserDto)
            .ChildRules(child =>
            {
                child.RuleLevelCascadeMode = CascadeMode.Stop;

                child.RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .WithMessage(Constants.Validation.User.EmailInvalidMessage)
                    .MustAsync(EmailMustNotExistAsync)
                    .WithMessage(Constants.Validation.User.EmailTakenMessage);

                child.RuleFor(x => x.FullName)
                    .SetValidator(_validator, Constants.Validation.RuleSet.Names);

                child.RuleFor(x => x.Password)
                    .SetValidator(_validator, Constants.Validation.RuleSet.Passwords);
            });
    }

    private async Task<bool> EmailMustNotExistAsync(string email, CancellationToken cancellationToken)
    {
        var emailExists = await _userRepository.EmailExistsAsync(email, cancellationToken);
        return !emailExists;
    }
}
