﻿using AutoFixture;
using Exadel.ReportHub.Handlers.Item.Create;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Item;
using Exadel.ReportHub.Tests.Abstracts;
using Moq;

namespace Exadel.ReportHub.Tests.Item.Create;

[TestFixture]
public class CreateItemHandlerTests : BaseTestFixture
{
    private Mock<IItemRepository> _itemRepositoryMock;
    private Mock<IClientRepository> _clientRepositoryMock;
    private CreateItemHandler _handler;

    [SetUp]
    public void Setup()
    {
        _itemRepositoryMock = new Mock<IItemRepository>();
        _clientRepositoryMock = new Mock<IClientRepository>();
        _handler = new CreateItemHandler(_itemRepositoryMock.Object, _clientRepositoryMock.Object, Mapper);
    }

    [Test]
    public async Task CreateItem_ValidRequest_ReturnsItemDTO()
    {
        // Arrange
        var createItemDto = Fixture.Create<CreateUpdateItemDTO>();
        var client = Fixture.Build<Data.Models.Client>()
            .With(x => x.Id, createItemDto.ClientId)
            .Create();

        _clientRepositoryMock
            .Setup(x => x.GetByIdAsync(createItemDto.ClientId, CancellationToken.None))
            .ReturnsAsync(client);

        // Act
        var createItemRequest = new CreateItemRequest(createItemDto);
        var result = await _handler.Handle(createItemRequest, CancellationToken.None);

        // Assert
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.IsError, Is.False);
        Assert.That(result.Value, Is.InstanceOf<ItemDTO>(), "Returned object should be an instance of ItemDTO");
        Assert.That(result.Value.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(result.Value.Name, Is.EqualTo(createItemDto.Name));
        Assert.That(result.Value.Description, Is.EqualTo(createItemDto.Description));
        Assert.That(result.Value.Price, Is.EqualTo(createItemDto.Price));
        Assert.That(result.Value.CurrencyId, Is.EqualTo(client.CurrencyId));
        Assert.That(result.Value.CurrencyCode, Is.EqualTo(client.CurrencyCode));

        _itemRepositoryMock.Verify(
            mock => mock.AddAsync(
                It.Is<Data.Models.Item>(
                    i => i.Name == createItemDto.Name &&
                         i.Description == createItemDto.Description &&
                         i.Price == createItemDto.Price &&
                         i.CurrencyId == client.CurrencyId &&
                         i.CurrencyCode == client.CurrencyCode &&
                         !i.IsDeleted),
                CancellationToken.None),
            Times.Once);
    }
}
