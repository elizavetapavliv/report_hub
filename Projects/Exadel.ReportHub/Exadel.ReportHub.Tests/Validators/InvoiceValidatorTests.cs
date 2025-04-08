using Exadel.ReportHub.Handlers.Validators;
using Exadel.ReportHub.SDK.DTOs.Invoice;
using Exadel.ReportHub.SDK.Enums;
using FluentValidation;
using FluentValidation.TestHelper;

namespace Exadel.ReportHub.Tests;

public class InvoiceValidatorTests
{
    private IValidator<CreateInvoiceDTO> _invoiceValidator;

    [SetUp]
    public void Setup()
    {
        _invoiceValidator = new InvoiceDtoValidator();
    }

    [Test]
    public async Task ValidateAsync_EverythingIsValid_NoErrorReturned()
    {
        // Arrange
        var invoice = new CreateInvoiceDTO
        {
            ClientId = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            InvoiceNumber = "INV123456",
            IssueDate = DateTime.UtcNow.AddDays(-5),
            DueDate = DateTime.UtcNow.AddDays(30),
            Amount = 1000.00m,
            PaymentStatus = PaymentStatus.Paid,
            Currency = "USD",
            BankAccountNumber = "123456789",
            ItemIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
        };

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.True);
        Assert.That(result.Errors, Has.Count.EqualTo(0));
    }

    [Test]
    [TestCase("")]
    [TestCase(null)]
    public async Task ValidateAsync_EverythingIsEmpty_ErrorReturned(string emptyStringOrNull)
    {
        // Arrange
        var invoice = new CreateInvoiceDTO
        {
            ClientId = Guid.Empty,
            CustomerId = Guid.Empty,
            InvoiceNumber = emptyStringOrNull,
            IssueDate = DateTime.MinValue,
            DueDate = DateTime.MinValue,
            Amount = 0,
            Currency = emptyStringOrNull,
            BankAccountNumber = emptyStringOrNull,
            ItemIds = new List<Guid>(),
        };

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Has.Count.EqualTo(9));
    }

    [Test]
    [TestCase("INV1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890")] // will cause length error
    [TestCase("123456789")] // will cause no "INV" at the start error
    public async Task ValidateAsync_InvoiceNumberIsInvalid_ErrorReturned(string invoiceNumber)
    {
        // Arrange
        var invoice = new CreateInvoiceDTO
        {
            ClientId = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            InvoiceNumber = invoiceNumber,
            IssueDate = DateTime.UtcNow.AddDays(-5),
            DueDate = DateTime.UtcNow.AddDays(30),
            Amount = 1000.00m,
            PaymentStatus = PaymentStatus.Unpaid,
            Currency = "USD",
            BankAccountNumber = "123456789",
            ItemIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
        };

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Has.Count.EqualTo(1));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo(nameof(CreateInvoiceDTO.InvoiceNumber)));
    }

    [Test]
    public async Task ValidateAsync_IssueDateIsInFuture_ErrorReturned()
    {
        // Arrange
        var invoice = new CreateInvoiceDTO
        {
            ClientId = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            InvoiceNumber = "INV20251023",
            IssueDate = DateTime.UtcNow.AddDays(5),
            DueDate = DateTime.UtcNow.AddDays(30),
            Amount = 1000.00m,
            PaymentStatus = PaymentStatus.Paid,
            Currency = "USD",
            BankAccountNumber = "123456789",
            ItemIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
        };

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Has.Count.EqualTo(1));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo(nameof(CreateInvoiceDTO.IssueDate)));
    }

    [Test]
    public async Task ValidateAsync_DueDateIsLessThenIssueDate_ErrorReturned()
    {
        // Arrange
        var invoice = new CreateInvoiceDTO
        {
            ClientId = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            InvoiceNumber = "INV20251023",
            IssueDate = DateTime.UtcNow.AddDays(-5),
            DueDate = DateTime.UtcNow.AddDays(-10),
            Amount = 1000.00m,
            PaymentStatus = PaymentStatus.Paid,
            Currency = "USD",
            BankAccountNumber = "123456789",
            ItemIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
        };

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Has.Count.EqualTo(1));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo(nameof(CreateInvoiceDTO.DueDate)));
    }

    [Test]
    public async Task ValidateAsync_AmountIsNegative_ErrorReturned()
    {
        // Arrange
        var invoice = new CreateInvoiceDTO
        {
            ClientId = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            InvoiceNumber = "INV20251023",
            IssueDate = DateTime.UtcNow.AddDays(-5),
            DueDate = DateTime.UtcNow.AddDays(30),
            Amount = -1000.00m,
            PaymentStatus = PaymentStatus.Paid,
            Currency = "USD",
            BankAccountNumber = "123456789",
            ItemIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
        };

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Has.Count.EqualTo(1));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo(nameof(CreateInvoiceDTO.Amount)));
    }

    [Test]
    [TestCase("US")] // will cause length error
    [TestCase("usd")] // will cause no uppercase error
    public async Task ValidateAsync_CurrencyCodeIsInvalid_ErrorReturned(string currency)
    {
        // Arrange
        var invoice = new CreateInvoiceDTO
        {
            ClientId = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            InvoiceNumber = "INV20251023",
            IssueDate = DateTime.UtcNow.AddDays(-5),
            DueDate = DateTime.UtcNow.AddDays(30),
            Amount = 1000.00m,
            PaymentStatus = PaymentStatus.Paid,
            Currency = currency,
            BankAccountNumber = "123456789",
            ItemIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
        };

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Has.Count.EqualTo(1));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo(nameof(CreateInvoiceDTO.Currency)));
    }

    [Test]
    public async Task ValidateAsync_PaymentStatusIsInvalid_ErrorReturned()
    {
        // Arrange
        var invoice = new CreateInvoiceDTO
        {
            ClientId = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            InvoiceNumber = "INV20251023",
            IssueDate = DateTime.UtcNow.AddDays(-5),
            DueDate = DateTime.UtcNow.AddDays(30),
            Amount = 1000.00m,
            PaymentStatus = (PaymentStatus)100,
            Currency = "USD",
            BankAccountNumber = "123456789",
            ItemIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
        };

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Has.Count.EqualTo(1));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo(nameof(CreateInvoiceDTO.PaymentStatus)));
    }
}
