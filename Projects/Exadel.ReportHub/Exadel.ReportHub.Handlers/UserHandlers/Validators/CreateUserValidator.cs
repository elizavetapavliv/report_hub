using Exadel.ReportHub.RA.Abstract;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.UserHandlers.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    private readonly IUserRepository _userRepository;

    public CreateUserValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(async (email, cancellationToken) => await EmailMustNotExistAsync(email, cancellationToken))
            .WithMessage(Constants.Validation.User.EmailTakenMessage);

        RuleFor(x => x.FullName)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(Constants.Validation.User.PasswordMinimumLength)
            .WithMessage(Constants.Validation.User.PasswordMinLengthMessage)
            .Matches("[A-Z]")
            .WithMessage(Constants.Validation.User.PasswordUppercaseMessage)
            .Matches("[a-z]")
            .WithMessage(Constants.Validation.User.PasswordLowercaseMessage)
            .Matches("[0-9]")
            .WithMessage(Constants.Validation.User.PasswordDigitMessage)
            .Matches("[^a-zA-Z0-9]")
            .WithMessage(Constants.Validation.User.PasswordSpecialCharacterMessage);
    }

    private async Task<bool> EmailMustNotExistAsync(string email, CancellationToken cancellationToken)
    {
        var emailAddress = await _userRepository.EmailExistsAsync(email, cancellationToken);
        return !emailAddress;
    }
}
