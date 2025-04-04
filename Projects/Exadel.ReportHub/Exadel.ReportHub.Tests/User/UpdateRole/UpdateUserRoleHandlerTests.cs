using AutoFixture;
using ErrorOr;
using Exadel.ReportHub.Handlers.User.UpdateRole;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.Tests.Abstracts;
using Moq;

namespace Exadel.ReportHub.Tests.User.UpdateRole;

[TestFixture]
public class UpdateUserRoleHandlerTests : BaseTestFixture
{
    private Mock<IUserRepository> _userRepositoryMock;
    private UpdateUserRoleHandler _handler;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new UpdateUserRoleHandler(_userRepositoryMock.Object);
    }

    [Test]
    public async Task UpdateUserRole_WhenUserExists_ReturnsUpdated()
    {
        // Arrange
        var user = Fixture.Create<Data.Models.User>();
        _userRepositoryMock
            .Setup(x => x.ExistsAsync(user.Id, CancellationToken.None))
            .ReturnsAsync(true);

        // Act
        var request = new UpdateUserRoleRequest(user.Id, user.Role);
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.IsError, Is.False);
        Assert.That(result.Value, Is.EqualTo(Result.Updated));

        _userRepositoryMock.Verify(
            x => x.UpdateRoleAsync(user.Id, user.Role, CancellationToken.None),
            Times.Once);
    }

    [Test]
    public async Task UpdateUserRole_WhenUserNotExists_ReturnsNotFound()
    {
        // Arrange
        var user = Fixture.Create<Data.Models.User>();
        _userRepositoryMock
            .Setup(x => x.ExistsAsync(user.Id, CancellationToken.None))
            .ReturnsAsync(false);

        // Act
        var request = new UpdateUserRoleRequest(user.Id, user.Role);
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.IsError, Is.True, "Should contains user not found Error");

        _userRepositoryMock.Verify(
            x => x.UpdateRoleAsync(user.Id, user.Role, CancellationToken.None),
            Times.Never);
    }
}
