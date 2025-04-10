using ErrorOr;
using Exadel.ReportHub.Common.Exceptions;
using Exadel.ReportHub.Common.Providers;
using Exadel.ReportHub.Handlers.User.UpdatePassword;
using Exadel.ReportHub.RA.Abstract;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Exadel.ReportHub.Tests;

public class UpdateUserPasswordHandlerTests
{
    private const string Password = "TestPassword123!";

    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IUserProvider> _userProviderMock;
    private UpdateUserPasswordHandler _handler;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userProviderMock = new Mock<IUserProvider>();
        _handler = new UpdateUserPasswordHandler(_userRepositoryMock.Object, _userProviderMock.Object);
    }

    [Test]
    public async Task UpdateUserPassword_AuthenticatedUser_ReturnsUpdated()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _userProviderMock
            .Setup(x => x.GetUserId())
            .Returns(userId);

        _userRepositoryMock
            .Setup(x => x.UpdatePasswordAsync(userId, It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(new UpdateUserPasswordRequest(Password), CancellationToken.None);

        // Assert
        Assert.That(result.IsError, Is.False);
        Assert.That(result.Value, Is.EqualTo(Result.Updated));
        _userProviderMock.Verify(x => x.GetUserId(), Times.Once);
        _userRepositoryMock.Verify(x => x.UpdatePasswordAsync(userId, It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None), Times.Once);
    }

    [Test]
    public async Task UpdateUserPassword_UnauthenticatedUser_ReturnsNotFound()
    {
        // Arrange
        _userProviderMock.Setup(x => x.GetUserId())
            .Throws(new HttpStatusCodeException(StatusCodes.Status401Unauthorized));

        // Act
        HttpStatusCodeException ex = null;
        try
        {
            await _handler.Handle(new UpdateUserPasswordRequest(Password), CancellationToken.None);
        }
        catch (HttpStatusCodeException e)
        {
            ex = e;
        }

        // Assert
        Assert.That(ex.StatusCode, Is.EqualTo(StatusCodes.Status401Unauthorized));
        _userProviderMock.Verify(x => x.GetUserId(), Times.Once);
        _userRepositoryMock.Verify(x => x.UpdatePasswordAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None), Times.Never);
    }
}
