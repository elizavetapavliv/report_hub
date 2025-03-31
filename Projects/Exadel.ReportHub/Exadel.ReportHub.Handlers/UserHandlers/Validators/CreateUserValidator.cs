using Exadel.ReportHub.RA.Abstract;
using FluentValidation;
using Consts = Exadel.ReportHub.Handlers.Constants.Validation.User;

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
            .NotEmpty().WithMessage(Consts.EmailRequiredMessage)
            .EmailAddress().WithMessage(Consts.InvalidEmailMessage)
            .MustAsync(async (email, cancellationToken) => await EmailMustNotExistAsync(email, cancellationToken)).WithMessage(Consts.EmailTakenMessage);

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage(Consts.FullNameRequiredMessage);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(Consts.PasswordRequiredMessage)
            .MinimumLength(Consts.PasswordMinimumLength).WithMessage(Consts.PasswordMinLengthMessage)
            .Matches("[A-Z]").WithMessage(Consts.PasswordUppercaseMessage)
            .Matches("[a-z]").WithMessage(Consts.PasswordLowercaseMessage)
            .Matches("[0-9]").WithMessage(Consts.PasswordDigitMessage)
            .Matches("[^a-zA-Z0-9]").WithMessage(Consts.PasswordSpecialCharacterMessage);
    }

    private async Task<bool> EmailMustNotExistAsync(string email, CancellationToken cancellationToken)
    {
        var emailAddress = await _userRepository.GetByEmailAsync(email, cancellationToken);
        return emailAddress == null;
    }
}
