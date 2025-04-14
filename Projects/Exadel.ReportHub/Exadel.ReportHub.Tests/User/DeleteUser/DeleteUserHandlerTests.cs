using AutoFixture;
using ErrorOr;
using Exadel.ReportHub.Handlers.User.DeleteUser;
using Exadel.ReportHub.RA.Abstract;
using Moq;

namespace Exadel.ReportHub.Tests;

public class DeleteUserHandlerTests
{
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IUserAssignmentRepository> _userAssignmentRepositoryMock;
    private DeleteUserHandler _handler;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userAssignmentRepositoryMock = new Mock<IUserAssignmentRepository>();
        _handler = new DeleteUserHandler(_userRepositoryMock.Object, _userAssignmentRepositoryMock.Object);
    }

    [Test]
    public async Task DeleteUser_UserExists_ReturnsDeleted()
    {
        var userId = Guid.NewGuid();
        var clientIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        _userRepositoryMock
            .Setup(repo => repo.ExistsAsync(userId, CancellationToken.None))
            .ReturnsAsync(true);

        _userAssignmentRepositoryMock
            .Setup(repo => repo.GetClientIdsByUserIdAsync(userId, CancellationToken.None))
            .ReturnsAsync(clientIds);

        var request = new DeleteUserRequest(userId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.IsError, Is.False);
        Assert.That(result.Value, Is.EqualTo(Result.Deleted));

        _userRepositoryMock.Verify(repo => repo.DeleteUserAsync(userId, CancellationToken.None), Times.Once);
        _userAssignmentRepositoryMock.Verify(repo => repo.DeleteUserAssignmentAsync(userId, clientIds, CancellationToken.None), Times.Once);
    }

    [Test]
    public async Task DeleteUser_UserDoesntExist_ReturnesNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new DeleteUserRequest(userId);
        _userRepositoryMock.Setup(x => x.ExistsAsync(userId, CancellationToken.None))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsTrue(result.IsError);
        Assert.That(result.FirstError.Type, Is.EqualTo(ErrorType.NotFound));
        _userRepositoryMock.Verify(x => x.ExistsAsync(userId, CancellationToken.None), Times.Once);
        _userRepositoryMock.Verify(x => x.DeleteUserAsync(userId, CancellationToken.None), Times.Never);
    }

    [Test]
    public async Task DeleteUser_UserExistsWithoutClientIds_DeletesUserOnly()
    {
        var userId = Guid.NewGuid();

        _userRepositoryMock
            .Setup(repo => repo.ExistsAsync(userId, CancellationToken.None))
            .ReturnsAsync(true);

        _userAssignmentRepositoryMock
            .Setup(repo => repo.GetClientIdsByUserIdAsync(userId, CancellationToken.None))
            .ReturnsAsync((IEnumerable<Guid>)null);

        var request = new DeleteUserRequest(userId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.IsError, Is.False);
        Assert.That(result.Value, Is.EqualTo(Result.Deleted));

        _userRepositoryMock.Verify(repo => repo.DeleteUserAsync(userId, CancellationToken.None), Times.Once);
        _userAssignmentRepositoryMock.Verify(repo => repo.DeleteUserAssignmentAsync(userId, It.IsAny<IEnumerable<Guid>>(), CancellationToken.None), Times.Never);
    }
}
