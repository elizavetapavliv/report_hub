using Exadel.ReportHub.Handlers.Item.Create;
using Exadel.ReportHub.SDK.DTOs.Item;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using Moq;

namespace Exadel.ReportHub.Tests.Item.Create;

[TestFixture]
public class CreateItemRequestValidatorTests
{
    private Mock<IValidator<CreateUpdateItemDTO>> _itemValidatorMock;
    private CreateItemRequestValidator _validator;

    [SetUp]
    public void Setup()
    {
        _itemValidatorMock = new Mock<IValidator<CreateUpdateItemDTO>>();
        _validator = new CreateItemRequestValidator(_itemValidatorMock.Object);
    }

    [Test]
    public async Task ValidateAsync_ValidRequest_NoErrors()
    {
        // Arrange
        var createItemDto = new CreateUpdateItemDTO();
        _itemValidatorMock.Setup(x => x.ValidateAsync(createItemDto, CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        var request = new CreateItemRequest(createItemDto);

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task ValidateAsync_EmptyItemDto_ErrorReturned()
    {
        // Arrange
        var request = new CreateItemRequest(null);

        // Act
        var result = await _validator.TestValidateAsync(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CreateItemDto)
            .WithErrorMessage("'Create Item Dto' must not be empty.");
    }
}
