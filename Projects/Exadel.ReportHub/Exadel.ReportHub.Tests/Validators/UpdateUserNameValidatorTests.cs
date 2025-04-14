using Exadel.ReportHub.Handlers.User.Create;
using Exadel.ReportHub.Handlers.User.UpdateName;
using Exadel.ReportHub.SDK.DTOs.User;
using FluentValidation;
using FluentValidation.TestHelper;

namespace Exadel.ReportHub.Tests;

public class UpdateUserNameValidatorTests
{
    private UpdateUserNameRequestValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new UpdateUserNameRequestValidator();
    }

    [Test]
    [TestCase("")]
    [TestCase(null)]
    public async Task ValidateAsync_FullNameIsEmpty_ErrorReturned(string fullname)
    {
        var userId = Guid.NewGuid();
        var createUserRequest = new UpdateUserNameRequest(userId, fullname);
        var result = await _validator.TestValidateAsync(createUserRequest);
        result.ShouldHaveValidationErrorFor(x => x.FullName)
            .WithErrorMessage("'Full Name' must not be empty.");
        Assert.That(result.Errors, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task ValidateAsync_FullNameExceedsMaxLength_ErrorReturned()
    {
        var userId = Guid.NewGuid();
        var maxLength = 101;
        var fullname = new string('x', maxLength);
        var createUserRequest = new UpdateUserNameRequest(userId, fullname);
        var result = await _validator.TestValidateAsync(createUserRequest);
        result.ShouldHaveValidationErrorFor(x => x.FullName)
            .WithErrorMessage($"The length of 'Full Name' must be 100 characters or fewer. You entered {maxLength} characters.");
        Assert.That(result.Errors, Has.Count.EqualTo(1));
    }
}
