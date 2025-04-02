using System.Runtime.InteropServices;
using System.Security;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.Validators;

public class PasswordValidator : AbstractValidator<SecureString>
{
    public PasswordValidator()
    {
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => SecureStringToString(x))
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

    private string SecureStringToString(SecureString secureString)
    {
        IntPtr bstr = IntPtr.Zero;
        try
        {
            bstr = Marshal.SecureStringToBSTR(secureString);
            return Marshal.PtrToStringBSTR(bstr);
        }
        finally
        {
            if (bstr != IntPtr.Zero)
            {
                Marshal.ZeroFreeBSTR(bstr);
            }
        }
    }
}
