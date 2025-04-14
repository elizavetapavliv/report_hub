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
        var setUserAssignmentDto = Fixture.Build<UpsertUserAssignmentDTO>().With(x => x.Role, role).Create();

        // Act
        var request = new UpsertUserAssignmentRequest(setUserAssignmentDto);
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.IsError, Is.False);
        Assert.That(result.Value, Is.EqualTo(Result.Updated));

        _userAssignmentRepositoryMock.Verify(
            x => x.UpsertAsync(It.IsAny<Data.Models.UserAssignment>(), CancellationToken.None),
            Times.Once);
    }
}
