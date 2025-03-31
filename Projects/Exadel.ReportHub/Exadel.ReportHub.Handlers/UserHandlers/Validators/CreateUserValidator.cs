using Exadel.ReportHub.RA.Abstract;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.UserHandlers.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    private const int MinimumPasswordLength = 8;

    private readonly IUserRepository _userRepository;

    public CreateUserValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.Email).NotEmpty()
            .MustAsync(async (email, cancellationToken) =>
        await _userRepository.GetByEmailAsync(email, cancellationToken) == null)
            .WithMessage("Email is taken")
            .EmailAddress();

        RuleFor(x => x.FullName).NotEmpty();

        RuleFor(x => x.Password).NotEmpty()
            .MinimumLength(MinimumPasswordLength).WithMessage("Password must be at least 8 characters long")
            .Matches("[A-Z]").WithMessage("Password must have at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must have at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must have at least one digit")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");
    }
}
