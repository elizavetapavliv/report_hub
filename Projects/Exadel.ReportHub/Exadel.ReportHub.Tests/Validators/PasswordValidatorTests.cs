using Exadel.ReportHub.Handlers;
using Exadel.ReportHub.Handlers.Validators;
using FluentValidation;
using FluentValidation.TestHelper;

namespace Exadel.ReportHub.Tests;

[TestFixture]
public class PasswordValidatorTests
{
    private IValidator<string> _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new PasswordValidator();
    }

    [Test]
    public async Task ValidateAsync_PasswordIsEmpty_ErrorReturned()
    {
        var result = await _validator.TestValidateAsync(string.Empty);
        result.ShouldHaveValidationErrorFor(password => password)
            .WithErrorMessage("Password must not be empty.");
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    [Test]
    [TestCase("Test1!", "Password must be at least 8 characters long.")]
    [TestCase("test1234!", Constants.Validation.User.PasswordUppercaseMessage)]
    [TestCase("TEST1234!", Constants.Validation.User.PasswordLowercaseMessage)]
    [TestCase("Testtest!", Constants.Validation.User.PasswordDigitMessage)]
    [TestCase("Test12345", Constants.Validation.User.PasswordSpecialCharacterMessage)]
    public async Task ValidateAsync_PasswordIsInvalid_ErrorReturned(string password, string expectedMessage)
    {
        var result = await _validator.TestValidateAsync(password);
        result.ShouldHaveValidationErrorFor(password => password)
            .WithErrorMessage(expectedMessage);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task ValidateAsync_PasswordIsValid_NoErrorReturned()
    {
        var password = "Test1234!";
        var result = await _validator.TestValidateAsync(password);
        result.ShouldNotHaveAnyValidationErrors();
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }
}
