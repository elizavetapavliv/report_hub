using Exadel.ReportHub.Handlers;
using Exadel.ReportHub.Handlers.Validators;
using FluentValidation;
using FluentValidation.TestHelper;

namespace Exadel.ReportHub.Tests.Validators;

public partial class StringValidatorTests
{
    [TestFixture]
    public class Password
    {
        private IValidator<string> _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new StringValidator();
        }

        [Test]
        public async Task ValidateAsync_PasswordIsEmpty_ErrorReturned()
        {
            // Arrange
            var password = string.Empty;

            // Act
            var result = await _validator.TestValidateAsync(password, options => options.IncludeRuleSets(Constants.Validation.RuleSet.Passwords));

            // Assert
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
            Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo($"'{nameof(Constants.Validation.Password)}' must not be empty."));
        }

        [TestCase("Test1!")]
        [TestCase("test1234!", Constants.Validation.Password.RequireUppercase)]
        [TestCase("TEST1234!", Constants.Validation.Password.RequireLowercase)]
        [TestCase("Testtest!", Constants.Validation.Password.RequireDigit)]
        [TestCase("Test12345", Constants.Validation.Password.RequireSpecialCharacter)]
        public async Task ValidateAsync_InvalidPassword_ShouldReturnExpectedError(string password, string expectedMessage = null)
        {
            // Arrange
            if (expectedMessage == null)
            {
                expectedMessage = GetPasswordLengthMessage(password.Length);
            }

            // Act
            var result = await _validator.TestValidateAsync(password, options =>
                options.IncludeRuleSets(Constants.Validation.RuleSet.Passwords));

            // Assert
            result.ShouldHaveValidationErrorFor(nameof(Constants.Validation.Password))
                .WithErrorMessage(expectedMessage);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task ValidateAsync_PasswordIsValid_NoErrorReturned()
        {
            // Arrange
            var password = "Test1234!";

            // Act
            var result = await _validator.TestValidateAsync(password, options => options.IncludeRuleSets(Constants.Validation.RuleSet.Passwords));

            // Assert
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Errors, Is.Empty);
        }

        private static string GetPasswordLengthMessage(int totalLength) =>
        $"The length of '{nameof(Constants.Validation.Password)}' must be at least 8 characters. You entered {totalLength} characters.";
    }

    [TestFixture]
    public class Name
    {
        private IValidator<string> _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new StringValidator();
        }

        [Test]
        public async Task ValidateAsync_ValidName_NoErrorReturned()
        {
            // Arrange
            var name = "John";

            // Act
            var result = await _validator.TestValidateAsync(name, options => options.IncludeRuleSets(Constants.Validation.RuleSet.Names));

            // Assert
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Errors, Is.Empty);
        }

        [Test]
        public async Task ValidateAsync_EmptyName_ErrorReturned()
        {
            // Arrange
            var name = string.Empty;

            // Act
            var result = await _validator.TestValidateAsync(name, options => options.IncludeRuleSets(Constants.Validation.RuleSet.Names));

            // Assert
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo("'Name' must not be empty."));
        }

        [Test]
        public async Task ValidateAsync_NameTooLong_ErrorReturned()
        {
            // Arrange
            var name = new string('A', Constants.Validation.Name.MaxLength + 1);

            // Act
            var result = await _validator.TestValidateAsync(name, options => options.IncludeRuleSets(Constants.Validation.RuleSet.Names));

            // Assert
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo($"The length of 'Name' must be {Constants.Validation.Name.MaxLength} characters or fewer. You entered {name.Length} characters."));
        }

        [Test]
        public async Task ValidateAsync_NameDoesNotStartWithCapital_ErrorReturned()
        {
            // Arrange
            var name = "john";

            // Act
            var result = await _validator.TestValidateAsync(name, options => options.IncludeRuleSets(Constants.Validation.RuleSet.Names));

            // Assert
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo(Constants.Validation.Name.MustStartWithCapital));
        }
    }

    [TestFixture]
    public class Country
    {
        private IValidator<string> _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new StringValidator();
        }

        [Test]
        public async Task ValidateAsync_ValidCountry_NoErrorReturned()
        {
            // Arrange
            var country = "Georgia";

            // Act
            var result = await _validator.TestValidateAsync(country, options => options.IncludeRuleSets(Constants.Validation.RuleSet.Countries));

            // Assert
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.Errors, Is.Empty);
        }

        [Test]
        public async Task ValidateAsync_EmptyCountry_ErrorReturned()
        {
            // Arrange
            var country = string.Empty;

            // Act
            var result = await _validator.TestValidateAsync(country, options => options.IncludeRuleSets(Constants.Validation.RuleSet.Countries));

            // Assert
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo("'Country' must not be empty."));
        }

        [Test]
        public async Task ValidateAsync_CountryTooLong_ErrorReturned()
        {
            // Arrange
            var country = new string('A', Constants.Validation.Country.MaxLength + 1);

            // Act
            var result = await _validator.TestValidateAsync(country, options => options.IncludeRuleSets(Constants.Validation.RuleSet.Countries));

            // Assert
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors[0].ErrorMessage,
                Is.EqualTo($"The length of 'Country' must be {Constants.Validation.Country.MaxLength} characters or fewer. You entered {country.Length} characters."));
        }

        [Test]
        public async Task ValidateAsync_CountryDoesNotStartWithCapital_ErrorReturned()
        {
            // Arrange
            var country = "georgia";

            // Act
            var result = await _validator.TestValidateAsync(country, options => options.IncludeRuleSets(Constants.Validation.RuleSet.Countries));

            // Assert
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo(Constants.Validation.Country.MustStartWithCapital));
        }
    }
}
