using AutoFixture;
using ErrorOr;
using Exadel.ReportHub.Handlers.User.UpdateActivity;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.Tests.Abstracts;
using Moq;

namespace Exadel.ReportHub.Tests.User.UpdateActivity;

[TestFixture]
public class UpdateUserActivityHandlerTest : BaseTestFixture
{
    private Mock<IUserRepository> _userRepositoryMock;
    private UpdateUserActivityHandler _handler;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new UpdateUserActivityHandler(_userRepositoryMock.Object);
    }

    [Test]
    public async Task UpdateUserActivity_WhenUserExists_ReturnsUpdated()
    {
        // Arrange
        var user = Fixture.Create<Data.Models.User>();
        _userRepositoryMock
            .Setup(x => x.ExistsAsync(user.Id, CancellationToken.None))
            .ReturnsAsync(true);

        // Act
        var request = new UpdateUserActivityRequest(user.Id, true);
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.IsError, Is.False);
        Assert.That(result.Value, Is.EqualTo(Result.Updated));

        _userRepositoryMock.Verify(
            x => x.UpdateActivityAsync(user.Id, true, CancellationToken.None),
            Times.Once);
    }

    [Test]
    public async Task UpdateUserActivity_WhenUserNotExists_ReturnsNotFound()
    {
        // Arrange
        var user = Fixture.Create<Data.Models.User>();
        _userRepositoryMock
            .Setup(x => x.ExistsAsync(user.Id, CancellationToken.None))
            .ReturnsAsync(false);

        // Act
        var request = new UpdateUserActivityRequest(user.Id, true);
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.IsError, Is.True, "Should contains user not found Error");

        _userRepositoryMock.Verify(
            x => x.UpdateActivityAsync(user.Id, true, CancellationToken.None),
            Times.Never);
    }
}
