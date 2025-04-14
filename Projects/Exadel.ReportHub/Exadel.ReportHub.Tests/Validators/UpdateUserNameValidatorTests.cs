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
        var createUserRequest = new UpdateUserNameRequest(fullname);
        var result = await _validator.TestValidateAsync(createUserRequest);
        result.ShouldHaveValidationErrorFor(x => x.FullName)
            .WithErrorMessage("'Full Name' must not be empty.");
        Assert.That(result.Errors, Has.Count.EqualTo(1));
    }
}
