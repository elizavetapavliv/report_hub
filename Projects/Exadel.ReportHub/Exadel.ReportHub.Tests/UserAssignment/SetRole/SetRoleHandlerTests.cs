using AutoFixture;
using ErrorOr;
using Exadel.ReportHub.Common.Exceptions;
using Exadel.ReportHub.Handlers;
using Exadel.ReportHub.Handlers.UserAssignment.SetRole;
using Exadel.ReportHub.Host.Mediatr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.UserAssignment;
using Exadel.ReportHub.SDK.Enums;
using Exadel.ReportHub.Tests.Abstracts;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Exadel.ReportHub.Tests.UserAssignment.SetRole;

[TestFixture]
public class SetRoleHandlerTests : BaseTestFixture
{
    private Mock<IUserAssignmentRepository> _userAssignmentRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IClientRepository> _clientRepositoryMock;
    private SetRoleHandler _handler;

    [SetUp]
    public void Setup()
    {
        _userAssignmentRepositoryMock = new Mock<IUserAssignmentRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _clientRepositoryMock = new Mock<IClientRepository>();
        _handler = new SetRoleHandler(_userAssignmentRepositoryMock.Object, Mapper);
    }

    [TestCase(UserRole.Regular)]
    [TestCase(UserRole.ClientAdmin)]
    [TestCase(UserRole.SuperAdmin)]
    public async Task SetRole_ValidRequest_ReturnsUpdated(UserRole role)
    {
        // Arrange
        var setUserAssignmentDto = Fixture.Build<SetUserAssignmentDTO>().With(x => x.Role, role).Create();

        _userRepositoryMock
            .Setup(x => x.ExistsAsync(setUserAssignmentDto.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _clientRepositoryMock
            .Setup(x => x.ExistsAsync(setUserAssignmentDto.ClientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validationBehavior = new RequestValidationBehavior<SetRoleRequest, ErrorOr<Updated>>(
            new List<SetRoleRequestValidator>()
            {
                new SetRoleRequestValidator(_userRepositoryMock.Object, _clientRepositoryMock.Object)
            });

        // Act
        var request = new SetRoleRequest(setUserAssignmentDto);
        var result = await validationBehavior.Handle(request, () =>
        {
            return _handler.Handle(request, CancellationToken.None);
        }, CancellationToken.None);

        // Assert
        Assert.That(result.IsError, Is.False);
        Assert.That(result.Value, Is.EqualTo(Result.Updated));

        _userAssignmentRepositoryMock.Verify(
            x => x.SetRoleAsync(It.IsAny<Data.Models.UserAssignment>(), CancellationToken.None),
            Times.Once);
    }

    [TestCase(UserRole.Regular)]
    [TestCase(UserRole.ClientAdmin)]
    [TestCase(UserRole.SuperAdmin)]
    public void SetRole_UserNotExist_ThrowsValidationException(UserRole role)
    {
        // Arrange
        var setUserAssignmentDto = Fixture.Build<SetUserAssignmentDTO>().With(x => x.Role, role).Create();

        _userRepositoryMock
            .Setup(x => x.ExistsAsync(setUserAssignmentDto.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _clientRepositoryMock
            .Setup(x => x.ExistsAsync(setUserAssignmentDto.ClientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validationBehavior = new RequestValidationBehavior<SetRoleRequest, ErrorOr<Updated>>(
            new List<SetRoleRequestValidator>()
            {
                new SetRoleRequestValidator(_userRepositoryMock.Object, _clientRepositoryMock.Object)
            });

        // Act
        var request = new SetRoleRequest(setUserAssignmentDto);

        var result = Assert.ThrowsAsync<HttpStatusCodeException>(async () => await validationBehavior.Handle(request, () =>
            {
                return _handler.Handle(request, CancellationToken.None);
            }, CancellationToken.None));

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        Assert.That(result.Errors, Has.Exactly(1).Items);
        Assert.That(result.Errors.First, Is.EqualTo(Constants.Validation.UserAssignment.UserNotExistMessage));

        _userAssignmentRepositoryMock.Verify(
            x => x.SetRoleAsync(It.IsAny<Data.Models.UserAssignment>(), CancellationToken.None),
            Times.Never);
    }

    [TestCase(UserRole.Regular)]
    [TestCase(UserRole.ClientAdmin)]
    [TestCase(UserRole.SuperAdmin)]
    public void SetRole_ClientNotExist_ThrowsValidationException(UserRole role)
    {
        // Arrange
        var setUserAssignmentDto = Fixture.Build<SetUserAssignmentDTO>().With(x => x.Role, role).Create();

        _userRepositoryMock
            .Setup(x => x.ExistsAsync(setUserAssignmentDto.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _clientRepositoryMock
            .Setup(x => x.ExistsAsync(setUserAssignmentDto.ClientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var validationBehavior = new RequestValidationBehavior<SetRoleRequest, ErrorOr<Updated>>(
            new List<SetRoleRequestValidator>()
            {
                new SetRoleRequestValidator(_userRepositoryMock.Object, _clientRepositoryMock.Object)
            });

        // Act
        var request = new SetRoleRequest(setUserAssignmentDto);

        var result = Assert.ThrowsAsync<HttpStatusCodeException>(async () => await validationBehavior.Handle(request, () =>
        {
            return _handler.Handle(request, CancellationToken.None);
        }, CancellationToken.None));

        // Assert
        Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        Assert.That(result.Errors, Has.Exactly(1).Items);
        Assert.That(result.Errors.First, Is.EqualTo(Constants.Validation.UserAssignment.ClientNotExistMessage));

        _userAssignmentRepositoryMock.Verify(
            x => x.SetRoleAsync(It.IsAny<Data.Models.UserAssignment>(), CancellationToken.None),
            Times.Never);
    }
}
