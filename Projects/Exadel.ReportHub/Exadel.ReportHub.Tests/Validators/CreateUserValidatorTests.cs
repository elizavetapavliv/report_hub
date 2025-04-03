using Exadel.ReportHub.Handlers;
using Exadel.ReportHub.Handlers.User.Create;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.User;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using static Duende.IdentityServer.Models.IdentityResources;

namespace Exadel.ReportHub.Tests.Validators;

[TestFixture]
public class CreateUserValidatorTests
{
    private IValidator<CreateUserRequest> _validator;
    private Mock<IUserRepository> _userRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _validator = new CreateUserRequestValidator(_userRepositoryMock.Object, new Mock<IValidator<string>>().Object);
    }

    [Test]
    public async Task ValidateAsync_FullNameIsEmpty_ErrorReturned()
    {
        var createUserRequest = new CreateUserRequest(new CreateUserDTO { FullName = string.Empty, Email = "test@gmail.com", Password = "Testpassword123!" });
        var result = await _validator.TestValidateAsync(createUserRequest);
        result.ShouldHaveValidationErrorFor(x => x.CreateUserDto.FullName)
            .WithErrorMessage("'Full Name' must not be empty.");
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task ValidateAsync_FullNameIsNotEmpty_NoErrorReturned()
    {
        var createUserRequest = new CreateUserRequest(new CreateUserDTO { FullName = "Test", Email = "test@gmail.com", Password = "Testpassword123!" });
        var result = await _validator.TestValidateAsync(createUserRequest);
        result.ShouldNotHaveValidationErrorFor(x => x.CreateUserDto.FullName);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task ValidateAsync_EmailIsEmpty_ErrorReturned()
    {
        var createUserRequest = new CreateUserRequest(new CreateUserDTO { FullName = "Test User", Email = string.Empty, Password = "Testpassword123!" });
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
        _userRepositoryMock.Setup(repo => repo.EmailExistsAsync("demo.user3@gmail.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var createUserRequest = new CreateUserRequest(new CreateUserDTO { FullName = "Test User", Email = "demo.user3@gmail.com", Password = "Testpassword123!" });

        var result = await _validator.TestValidateAsync(createUserRequest);
        result.ShouldHaveValidationErrorFor(x => x.CreateUserDto.Email)
            .WithErrorMessage(Constants.Validation.User.EmailTakenMessage);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task ValidateAsync_EmailIsValid_NoErrorReturned()
    {
        var createUserRequest = new CreateUserRequest(new CreateUserDTO { FullName = "Test", Email = "test@gmail.com", Password = "Testpassword123!" });
        var result = await _validator.TestValidateAsync(createUserRequest);
        result.ShouldNotHaveValidationErrorFor(x => x.CreateUserDto.Email);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }
}
