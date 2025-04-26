using Exadel.ReportHub.Handlers.Item.Update;
using Exadel.ReportHub.SDK.DTOs.Item;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using Moq;

namespace Exadel.ReportHub.Tests.Item.Update;

[TestFixture]
public class UpdateItemRequestValidatorTests
{
    private Mock<IValidator<CreateUpdateItemDTO>> _itemValidatorMock;
    private UpdateItemRequestValidator _validator;

    [SetUp]
    public void Setup()
    {
        _itemValidatorMock = new Mock<IValidator<CreateUpdateItemDTO>>();
        _validator = new UpdateItemRequestValidator(_itemValidatorMock.Object);
    }

    [Test]
    public async Task ValidateAsync_ValidRequest_NoErrors()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var updateItemDto = new CreateUpdateItemDTO();

        _itemValidatorMock.Setup(x => x.ValidateAsync(updateItemDto, CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // Act
        var request = new UpdateItemRequest(itemId, updateItemDto);
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task ValidateAsync_EmptyItemId_ErrorReturned()
    {
        // Arrange
        var itemId = Guid.Empty;
        var updateItemDto = new CreateUpdateItemDTO();

        _itemValidatorMock.Setup(x => x.ValidateAsync(updateItemDto, CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        // Act
        var request = new UpdateItemRequest(itemId, updateItemDto);
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("'Id' must not be empty.");
    }

    [Test]
    public async Task ValidateAsync_EmptyItemDto_ErrorReturned()
    {
        // Arrange
        var itemId = Guid.NewGuid();

        // Act
        var request = new UpdateItemRequest(itemId, null);
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UpdateItemDto)
            .WithErrorMessage("'Update Item Dto' must not be empty.");
    }
}
