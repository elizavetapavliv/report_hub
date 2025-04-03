using Exadel.ReportHub.Common;

namespace Exadel.ReportHub.Tests.Common;

[TestFixture]
public class PasswordHasherTests
{
    [Test]
    public void HashPassword_ShouldReturn_HashedAndSaltedValue()
    {
        var password = "TestPassword123!";
        var (hashedPassword, salt) = PasswordHasher.CreatePasswordHash(password);
        Assert.IsNotNull(hashedPassword);
        Assert.IsNotNull(salt);
        Assert.That(hashedPassword, Is.Not.EqualTo(password));
    }

    [Test]
    public void GetPasswordHash_ShouldReturn_HashedValue()
    {
        var password = "TestPassword123!";
        var (hashedPassword, salt) = PasswordHasher.CreatePasswordHash(password);
        var hashedValue = PasswordHasher.GetPasswordHash(password, salt);
        Assert.That(hashedValue, Is.EqualTo(hashedPassword));
    }
}
