﻿using AutoFixture;
using ErrorOr;
using Exadel.ReportHub.Handlers.UserAssignment.Upsert;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.UserAssignment;
using Exadel.ReportHub.SDK.Enums;
using Exadel.ReportHub.Tests.Abstracts;
using Moq;

namespace Exadel.ReportHub.Tests.UserAssignment.Upsert;

[TestFixture]
public class UpsertUserAssignmentHandlerTests : BaseTestFixture
{
    private Mock<IUserAssignmentRepository> _userAssignmentRepositoryMock;
    private UpsertUserAssignmentHandler _handler;

    [SetUp]
    public void Setup()
    {
        _userAssignmentRepositoryMock = new Mock<IUserAssignmentRepository>();
        _handler = new UpsertUserAssignmentHandler(_userAssignmentRepositoryMock.Object, Mapper);
    }

    [TestCase(UserRole.Regular)]
    [TestCase(UserRole.ClientAdmin)]
    [TestCase(UserRole.SuperAdmin)]
    public async Task UpsertUserAssignment_ValidRequest_ReturnsUpdated(UserRole role)
    {
        // Arrange
        var upsertUserAssignmentDto = Fixture.Build<UpsertUserAssignmentDTO>().With(x => x.Role, role).Create();
        var userAssignment = Mapper.Map<Data.Models.UserAssignment>(upsertUserAssignmentDto);

        // Act
        var request = new UpsertUserAssignmentRequest(upsertUserAssignmentDto);
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.IsError, Is.False);
        Assert.That(result.Value, Is.EqualTo(Result.Updated));

        _userAssignmentRepositoryMock.Verify(
            x => x.UpsertAsync(It.Is<Data.Models.UserAssignment>(
                ua => ua.UserId == userAssignment.UserId &&
                ua.ClientId == userAssignment.ClientId &&
                ua.Role == userAssignment.Role), CancellationToken.None),
            Times.Once);
    }
}
