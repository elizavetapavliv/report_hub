using Exadel.ReportHub.Common;

namespace Exadel.ReportHub.Tests.Common;

[TestFixture]
public class PasswordHasherTests
{
    [Test]
    public void CreatePasswordHash_ShouldGenerateUniqueHashAndSaltForEachCall()
    {
        var password = "TestPassword123!";
        var expectedHash = "I1cb7xDEm6ZofJLsd9PXqCGOdrA/xO1SI+tm1T6yjCEKzO/iNLMdAOvcRLdMcZGWTTM0MVt7tFysJHS7t0kaXA==";
        var expectedSalt = "aGVsbG9Xb3JsZA==";
        var (hashedPassword, salt) = PasswordHasher.CreatePasswordHash(password);
        Assert.That(hashedPassword, Is.Not.Null);
        Assert.That(hashedPassword, Is.Not.EqualTo(expectedHash));
        Assert.That(salt, Is.Not.Null);
        Assert.That(salt, Is.Not.EqualTo(expectedSalt));
    }

    [Test]
    public void GetPasswordHash_ShouldReturnHashedValue()
    {
        var password = "TestPassword123!";
        var expectedSalt = "aGVsbG9Xb3JsZA==";
        var expectedHashedPassword = "U97MW3kDru47iKqVXDCy8+bamGNa8jCoI28amjFozwzQMItKxzilV43JhvYwRoxMJtKNc/8jHD3iuDuj7xFVnA==";
        var hashedValue = PasswordHasher.GetPasswordHash(password, expectedSalt);
        Assert.That(hashedValue, Is.Not.Null);
        Assert.That(hashedValue, Is.EqualTo(expectedHashedPassword));
    }
}
