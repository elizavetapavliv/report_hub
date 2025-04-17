using AutoFixture;
using Exadel.ReportHub.Handlers.Client.Create;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Client;
using Exadel.ReportHub.Tests.Abstracts;
using Moq;

namespace Exadel.ReportHub.Tests.Client.Create;

[TestFixture]
public class CreateClientHandlerTests : BaseTestFixture
{
    private Mock<IClientRepository> _clientRepositoryMock;
    private CreateClientHandler _handler;

    [SetUp]
    public void Setup()
    {
        _clientRepositoryMock = new Mock<IClientRepository>();
        _handler = new CreateClientHandler(_clientRepositoryMock.Object, Mapper);
    }

    [Test]
    public async Task CreateClient_ValidRequest_ReturnsClientDTO()
    {
        // Arrange
        var createClientDto = Fixture.Create<CreateClientDTO>();

        // Act
        var createClientRequest = new CreateClientRequest(createClientDto);
        var result = await _handler.Handle(createClientRequest, CancellationToken.None);

        // Assert
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.IsError, Is.False);
        Assert.That(result.Value, Is.InstanceOf<ClientDTO>(), "Returned object should be an instance of ClientDTO");
        Assert.That(result.Value.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(result.Value.Name, Is.EqualTo(createClientDto.Name));

        _clientRepositoryMock.Verify(
            mock => mock.AddAsync(
                It.Is<Data.Models.Client>(
                    u => u.Name == createClientDto.Name),
                CancellationToken.None),
            Times.Once);
    }

    [Test]
    public async Task CreateClient_ClintAlreadyExists_ReturnsNotFoundError()
    {
        // Arrange
        var createClientDto = Fixture.Create<CreateClientDTO>();
        _clientRepositoryMock
            .Setup(x => x.NameExistsAsync(createClientDto.Name, CancellationToken.None))
            .ReturnsAsync(true);

        // Act
        var createClientRequest = new CreateClientRequest(createClientDto);
        var result = await _handler.Handle(createClientRequest, CancellationToken.None);

        // Assert;
        Assert.That(result.Errors, Has.Count.EqualTo(1));

        _clientRepositoryMock.Verify(
            mock => mock.AddAsync(
                It.IsAny<Data.Models.Client>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
