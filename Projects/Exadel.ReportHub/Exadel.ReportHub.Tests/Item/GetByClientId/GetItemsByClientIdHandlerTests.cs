using AutoFixture;
using Exadel.ReportHub.Handlers.Item.GetByClientId;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.Tests.Abstracts;
using Moq;

namespace Exadel.ReportHub.Tests.Item.GetByClientId;

[TestFixture]
public class GetItemsByClientIdHandlerTests : BaseTestFixture
{
    private Mock<IItemRepository> _itemRepositoryMock;
    private GetItemsByClientIdHandler _handler;

    [SetUp]
    public void Setup()
    {
        _itemRepositoryMock = new Mock<IItemRepository>();
        _handler = new GetItemsByClientIdHandler(_itemRepositoryMock.Object, Mapper);
    }

    [Test]
    public async Task Handle_ClientHasItems_ReturnsItemDTOs()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var items = Fixture.Build<Data.Models.Item>().With(x => x.ClientId, clientId).CreateMany(2).ToList();

        _itemRepositoryMock
            .Setup(x => x.GetByClientIdAsync(clientId, CancellationToken.None))
            .ReturnsAsync(items);

        // Act
        var request = new GetItemsByClientIdRequest(clientId);
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.IsError, Is.False);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value, Has.Exactly(2).Items);

        Assert.That(result.Value[0].Id, Is.EqualTo(items[0].Id));
        Assert.That(result.Value[0].ClientId, Is.EqualTo(clientId));
        Assert.That(result.Value[0].Name, Is.EqualTo(items[0].Name));
        Assert.That(result.Value[0].Description, Is.EqualTo(items[0].Description));
        Assert.That(result.Value[0].Price, Is.EqualTo(items[0].Price));
        Assert.That(result.Value[0].CurrencyId, Is.EqualTo(items[0].CurrencyId));
        Assert.That(result.Value[0].CurrencyCode, Is.EqualTo(items[0].CurrencyCode));

        Assert.That(result.Value[1].Id, Is.EqualTo(items[1].Id));
        Assert.That(result.Value[1].ClientId, Is.EqualTo(clientId));
        Assert.That(result.Value[1].Name, Is.EqualTo(items[1].Name));
        Assert.That(result.Value[1].Description, Is.EqualTo(items[1].Description));
        Assert.That(result.Value[1].Price, Is.EqualTo(items[1].Price));
        Assert.That(result.Value[1].CurrencyId, Is.EqualTo(items[1].CurrencyId));
        Assert.That(result.Value[1].CurrencyCode, Is.EqualTo(items[1].CurrencyCode));

        _itemRepositoryMock.Verify(
            x => x.GetByClientIdAsync(clientId, CancellationToken.None),
            Times.Once);
    }

    [Test]
    public async Task Handle_ClientHasNoItems_ReturnsEmptyList()
    {
        // Arrange
        var clientId = Guid.NewGuid();

        _itemRepositoryMock
            .Setup(x => x.GetByClientIdAsync(clientId, CancellationToken.None))
            .ReturnsAsync(new List<Data.Models.Item>());

        // Act
        var request = new GetItemsByClientIdRequest(clientId);
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.IsError, Is.False);
        Assert.That(result.Value, Is.Empty);

        _itemRepositoryMock.Verify(
            x => x.GetByClientIdAsync(clientId, CancellationToken.None),
            Times.Once);
    }
}
