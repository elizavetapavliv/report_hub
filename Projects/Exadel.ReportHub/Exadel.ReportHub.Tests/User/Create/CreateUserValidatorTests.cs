using Exadel.ReportHub.Common;
using Exadel.ReportHub.Handlers;
using Exadel.ReportHub.Handlers.User.Create;
using Exadel.ReportHub.Handlers.Validators;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.User;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using static Duende.IdentityServer.Models.IdentityResources;

namespace Exadel.ReportHub.Tests.User.Create;

[TestFixture]
public class CreateUserValidatorTests
{
    private IValidator<CreateUserRequest> _validator;
    private Mock<IUserRepository> _mockUserRepository;

    [SetUp]
    public void Setup()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        var passwordValidator = new PasswordValidator();
        _validator = new CreateUserRequestValidator(_mockUserRepository.Object, passwordValidator);
    }

    [Test]
    public async Task FullName_ShouldHaveError_WhenEmpty()
    {
        var createUserRequest = new CreateUserRequest(new CreateUserDTO { FullName = string.Empty, Email = "test@gmail.com", Password = "Testpassword123!" });
        var result = await _validator.TestValidateAsync(createUserRequest);
        result.ShouldHaveValidationErrorFor(x => x.CreateUserDto.FullName);
    }

    [Test]
    public async Task Email_ShouldHaveError_WhenEmpty()
    {
        var createUserRequest = new CreateUserRequest(new CreateUserDTO { FullName = "Test User", Email = string.Empty, Password = "Testpassword123!" });
        var result = await _validator.TestValidateAsync(createUserRequest);
        result.ShouldHaveValidationErrorFor(x => x.CreateUserDto.Email);
    }

    [Test]
    public async Task Email_ShouldHaveError_WhenInvalid()
    {
        var createUserRequest = new CreateUserRequest(new CreateUserDTO { FullName = "Test User", Email = "invalid-email", Password = "Testpassword123!" });
        var result = await _validator.TestValidateAsync(createUserRequest);
        result.ShouldHaveValidationErrorFor(x => x.CreateUserDto.Email)
            .WithErrorMessage(Constants.Validation.User.EmailInvalidMessage);
    }

    [Test]
    public async Task Email_ShouldHaveError_WhenTaken()
    {
        _mockUserRepository.Setup(repo => repo.EmailExistsAsync("demo.user3@gmail.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var createUserRequest = new CreateUserRequest(new CreateUserDTO { FullName = "Test User", Email = "demo.user3@gmail.com", Password = "Testpassword123!" });

        var result = await _validator.TestValidateAsync(createUserRequest);
        result.ShouldHaveValidationErrorFor(x => x.CreateUserDto.Email)
            .WithErrorMessage(Constants.Validation.User.EmailTakenMessage);
    }

    [Test]
    public async Task Password_ShouldHaveError_WhenEmpty()
    {
        var createUserRequest = new CreateUserRequest(new CreateUserDTO { FullName = "Test User", Email = "demo.user4@gmail.com", Password = string.Empty });
        var result = await _validator.TestValidateAsync(createUserRequest);
        result.ShouldHaveValidationErrorFor(x => x.CreateUserDto.Password);
    }

    [TestCase("Test1!", "Password must be at least 8 characters long.")]
    [TestCase("test1234!", Constants.Validation.User.PasswordUppercaseMessage)]
    [TestCase("TEST1234!", Constants.Validation.User.PasswordLowercaseMessage)]
    [TestCase("Testtest!", Constants.Validation.User.PasswordDigitMessage)]
    [TestCase("Test12345", Constants.Validation.User.PasswordSpecialCharacterMessage)]
    public async Task Password_ShouldHaveError_WhenInvalid(string password, string expectedMessage)
    {
        var createUserRequest = new CreateUserRequest(new CreateUserDTO { FullName = "Test User", Email = "demo.user4@gmail.com", Password = password });
        var result = await _validator.TestValidateAsync(createUserRequest);
        result.ShouldHaveValidationErrorFor(x => x.CreateUserDto.Password)
            .WithErrorMessage(expectedMessage);
    }
}
