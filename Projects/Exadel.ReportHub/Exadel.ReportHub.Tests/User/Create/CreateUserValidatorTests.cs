using Exadel.ReportHub.Handlers;
using Exadel.ReportHub.Handlers.User.Create;
using Exadel.ReportHub.Handlers.Validators;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.User;
using FluentValidation.TestHelper;
using Moq;

namespace Exadel.ReportHub.Tests.User.Create;

[TestFixture]
public class CreateUserValidatorTests
{
    private CreateUserRequestValidator _validator;
    private Mock<IUserRepository> _userRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        var propertyValidator = new PropertyValidator();

        _validator = new CreateUserRequestValidator(_userRepositoryMock.Object, propertyValidator);
    }

    [Test]
    public async Task ValidateAsync_FullNameIsEmpty_ErrorReturned()
    {
        var createUserRequest = new CreateUserRequest(new CreateUserDTO { FullName = string.Empty, Email = "test@gmail.com", Password = "Testpassword123!" });
        var result = await _validator.TestValidateAsync(createUserRequest);
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage($"'{nameof(Constants.Validation.Name)}' must not be empty.");
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task ValidateAsync_FullNameIsNotEmpty_NoErrorReturned()
    {
        var createUserRequest = new CreateUserRequest(new CreateUserDTO { FullName = "Test", Email = "test@gmail.com", Password = "Testpassword123!" });
        var result = await _validator.TestValidateAsync(createUserRequest);
        result.ShouldNotHaveAnyValidationErrors();
        Assert.That(result.Errors, Is.Empty);
    }

    [Test]
    public async Task ValidateAsync_FullNameExceedsMaxLength_ErrorReturned()
    {
        var maxLength = 101;
        var fullname = new string('x', maxLength);
        var createUserRequest = new CreateUserRequest(new CreateUserDTO { FullName = fullname, Email = "test@gmail.com", Password = "Testpassword123!" });
        var result = await _validator.TestValidateAsync(createUserRequest);
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage($"The length of '{nameof(Constants.Validation.Name)}' must be 100 characters or fewer. You entered {maxLength} characters.");
        Assert.That(result.Errors, Has.Count.EqualTo(1));
    }

    [Test]
    [TestCase("")]
    [TestCase(null)]
    public async Task ValidateAsync_EmailIsNullOrEmpty_ErrorReturned(string emailError)
    {
        var createUserRequest = new CreateUserRequest(new CreateUserDTO { FullName = "Test User", Email = emailError, Password = "Testpassword123!" });
        var result = await _validator.TestValidateAsync(createUserRequest);
        result.ShouldHaveValidationErrorFor(x => x.CreateUserDto.Email)
            .WithErrorMessage("'Email' must not be empty.");
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task ValidateAsync_EmailIsInvalid_ErrorReturned()
    {
        var createUserRequest = new CreateUserRequest(new CreateUserDTO { FullName = "Test User", Email = "invalid-email", Password = "Testpassword123!" });
        var result = await _validator.TestValidateAsync(createUserRequest);
        result.ShouldHaveValidationErrorFor(x => x.CreateUserDto.Email)
            .WithErrorMessage(Constants.Validation.User.EmailInvalidMessage);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task ValidateAsync_EmailIsTaken_ErrorReturned()
    {
        _userRepositoryMock.Setup(repo => repo.EmailExistsAsync("demo.user3@gmail.com", CancellationToken.None))
            .ReturnsAsync(true);

        var createUserRequest = new CreateUserRequest(new CreateUserDTO { FullName = "Test User", Email = "demo.user3@gmail.com", Password = "Testpassword123!" });

        var result = await _validator.TestValidateAsync(createUserRequest);
        result.ShouldHaveValidationErrorFor(x => x.CreateUserDto.Email)
            .WithErrorMessage(Constants.Validation.User.EmailTakenMessage);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task ValidateAsync_PasswordIsInvalid_ErrorReturned()
    {
        var createUserRequest = new CreateUserRequest(new CreateUserDTO { FullName = "Test", Email = "testemail@gmail.com", Password = "Password1" });
        var result = await _validator.TestValidateAsync(createUserRequest);
        result.ShouldHaveAnyValidationError()
            .WithErrorMessage(Constants.Validation.Password.SpecialCharacterMessage);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }
}
