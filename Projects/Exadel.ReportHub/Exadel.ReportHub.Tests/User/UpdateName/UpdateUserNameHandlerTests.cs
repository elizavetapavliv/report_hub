using ErrorOr;
using Exadel.ReportHub.Common.Providers;
using Exadel.ReportHub.Handlers.User.UpdateName;
using Exadel.ReportHub.RA.Abstract;
using Moq;

namespace Exadel.ReportHub.Tests;

public class UpdateUserNameHandlerTests
{
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IUserProvider> _userProviderMock;
    private UpdateUserNameHandler _handler;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userProviderMock = new Mock<IUserProvider>();
        _handler = new UpdateUserNameHandler(_userRepositoryMock.Object, _userProviderMock.Object);
    }

    [Test]
    public async Task UpdateUserName_ValidRequest_ReturnsUpdated()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var newName = "New Name";
        var request = new UpdateUserNameRequest(newName);
        _userProviderMock
            .Setup(x => x.GetUserId())
            .Returns(userId);
        _userRepositoryMock
            .Setup(x => x.UpdateNameAsync(userId, newName, CancellationToken.None))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.IsError, Is.False);
        Assert.That(result.Value, Is.EqualTo(Result.Updated));

        _userProviderMock.Verify(x => x.GetUserId(), Times.Once);
        _userRepositoryMock.Verify(x => x.UpdateNameAsync(userId, newName, CancellationToken.None), Times.Once);
    }
}
