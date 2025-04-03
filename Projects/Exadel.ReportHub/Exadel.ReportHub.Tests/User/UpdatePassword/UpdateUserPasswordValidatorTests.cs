using Exadel.ReportHub.Handlers;
using Exadel.ReportHub.Handlers.User.Create;
using Exadel.ReportHub.Handlers.User.UpdatePassword;
using Exadel.ReportHub.Handlers.Validators;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.User;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;

namespace Exadel.ReportHub.Tests;

public class UpdateUserPasswordValidatorTests
{
    private IValidator<UpdateUserPasswordRequest> _validator;

    [SetUp]
    public void Setup()
    {
        var passwordValidator = new PasswordValidator();
        _validator = new UpdateUserPasswordRequestValidator(passwordValidator);
    }

    [Test]
    [TestCase("Test1!", "Password must be at least 8 characters long.")]
    [TestCase("test1234!", Constants.Validation.User.PasswordUppercaseMessage)]
    [TestCase("TEST1234!", Constants.Validation.User.PasswordLowercaseMessage)]
    [TestCase("Testtest!", Constants.Validation.User.PasswordDigitMessage)]
    [TestCase("Test12345", Constants.Validation.User.PasswordSpecialCharacterMessage)]
    public async Task Password_ShouldHaveError_WhenInvalid(string password, string expectedMessage)
    {
        var createUserRequest = new UpdateUserPasswordRequest(password);
        var result = await _validator.TestValidateAsync(createUserRequest);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage(expectedMessage);
    }
}
