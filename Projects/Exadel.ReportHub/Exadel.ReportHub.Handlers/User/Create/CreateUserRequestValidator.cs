using Exadel.ReportHub.Handlers.Validators;
using Exadel.ReportHub.RA.Abstract;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.User.Create;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordValidator _passwordValidator;

    public CreateUserRequestValidator(IUserRepository userRepository, PasswordValidator passwordValidator)
    {
        _userRepository = userRepository;
        _passwordValidator = passwordValidator;
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
                    .NotEmpty();

                child.RuleFor(x => x.Password)
                    .SetValidator(_passwordValidator);
            });
    }

    private async Task<bool> EmailMustNotExistAsync(string email, CancellationToken cancellationToken)
    {
        var emailExists = await _userRepository.EmailExistsAsync(email, cancellationToken);
        return !emailExists;
    }
}
