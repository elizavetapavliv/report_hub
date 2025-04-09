using AutoFixture;
using Exadel.ReportHub.Handlers;
using Exadel.ReportHub.Handlers.Validators;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Invoice;
using Exadel.ReportHub.SDK.Enums;
using Exadel.ReportHub.Tests.Abstracts;
using FluentValidation;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;

namespace Exadel.ReportHub.Tests;

public class InvoiceValidatorTests : BaseTestFixture
{
    private IValidator<CreateInvoiceDTO> _invoiceValidator;
    private Mock<IClientRepository> _clientRepositoryMock;
    private Mock<ICustomerRepository> _customerRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _clientRepositoryMock = new Mock<IClientRepository>();
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _invoiceValidator = new InvoiceValidator(_customerRepositoryMock.Object, _clientRepositoryMock.Object);
    }

    // Not Empty Tests
    [Test]
    public async Task ValidateAsync_EverythingIsValid_NoErrorReturned()
    {
        // Arrange
        var invoice = GetValidInvoice();

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.True);
        Assert.That(result.Errors, Has.Count.EqualTo(0));
    }

    [Test]
    public async Task ValidateAsync_ClientIdIsEmpty_ErrorReturned()
    {
        // Arrange
        var invoice = GetValidInvoice();
        invoice.ClientId = Guid.Empty;

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo(nameof(CreateInvoiceDTO.ClientId)));
    }

    [Test]
    public async Task ValidateAsync_CustomerIdIsEmpty_ErrorReturned()
    {
        // Arrange
        var invoice = GetValidInvoice();
        invoice.CustomerId = Guid.Empty;

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo(nameof(CreateInvoiceDTO.CustomerId)));
    }

    [Test]
    [TestCase("")]
    [TestCase(null)]
    public async Task ValidateAsync_InvoiceNumberIsNullOrEmpty_ErrorReturned(string value)
    {
        // Arrange
        var invoice = GetValidInvoice();
        invoice.InvoiceNumber = value;

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
        Assert.That(result.Errors.Any(x => x.PropertyName == nameof(CreateInvoiceDTO.InvoiceNumber)), Is.True);
    }

    [Test]
    public async Task ValidateAsync_DueDateIsDefault_ErrorReturned()
    {
        // Arrange
        var invoice = GetValidInvoice();
        invoice.DueDate = DateTime.MinValue;

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo(nameof(CreateInvoiceDTO.DueDate)));
    }

    [Test]
    public async Task ValidateAsync_IssueDateIsDefault_ErrorReturned()
    {
        // Arrange
        var invoice = GetValidInvoice();
        invoice.IssueDate = DateTime.MinValue;

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo(nameof(CreateInvoiceDTO.IssueDate)));
    }

    [Test]
    public async Task ValidateAsync_AmountIsZero_ErrorReturned()
    {
        // Arrange
        var invoice = GetValidInvoice();
        invoice.Amount = 0m;

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo(nameof(CreateInvoiceDTO.Amount)));
    }

    [Test]
    [TestCase("")]
    [TestCase(null)]
    public async Task ValidateAsync_CurrencyIsNullOrEmpty_ErrorReturned(string value)
    {
        // Arrange
        var invoice = GetValidInvoice();
        invoice.Currency = value;

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
        Assert.That(result.Errors.Any(x => x.PropertyName == nameof(CreateInvoiceDTO.Currency)), Is.True);
    }

    [Test]
    [TestCase("")]
    [TestCase(null)]
    public async Task ValidateAsync_BankAccountNumberIsNullOrEmpty_ErrorReturned(string value)
    {
        // Arrange
        var invoice = GetValidInvoice();
        invoice.BankAccountNumber = value;

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
        Assert.That(result.Errors.Any(x => x.PropertyName == nameof(CreateInvoiceDTO.BankAccountNumber)), Is.True);
    }

    [Test]
    public async Task ValidateAsync_ItemIdsIsEmpty_ErrorReturned()
    {
        // Arrange
        var invoice = GetValidInvoice();
        invoice.ItemIds = new List<Guid>();

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo(nameof(CreateInvoiceDTO.ItemIds)));
    }

    // Client and Customer tests
    [Test]
    public async Task ValidateAsync_ClientExists_ErrorReturned()
    {
        // Arrange
        var invoice = GetValidInvoice();
        var clientId = Guid.NewGuid();
        invoice.ClientId = clientId;
        _clientRepositoryMock.Setup(x => x.ExistsAsync(clientId, CancellationToken.None))
            .ReturnsAsync(false);

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo(nameof(CreateInvoiceDTO.ClientId)));
        Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo(Handlers.Constants.Validation.Invoice.ClientDoesntExistsErrorMessage));
    }

    [Test]
    public async Task ValidateAsync_CustomerExists_ErrorReturned()
    {
        // Arrange
        var invoice = GetValidInvoice();
        var customerId = Guid.NewGuid();
        invoice.CustomerId = customerId;
        _customerRepositoryMock.Setup(x => x.ExistsAsync(customerId, CancellationToken.None))
            .ReturnsAsync(false);

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo(nameof(CreateInvoiceDTO.CustomerId)));
        Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo(Handlers.Constants.Validation.Invoice.CustomerDoesntExistsErrorMessage));
    }

    // InvoiceNumber tests
    [Test]
    public async Task ValidateAsync_InvoiceNumberBigLength_ErrorReturned()
    {
        // Arrange
        var invoice = GetValidInvoice();
        invoice.InvoiceNumber = "INV122334323423434324233";

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo(nameof(CreateInvoiceDTO.InvoiceNumber)));
    }

    [Test]
    public async Task ValidateAsync_InvoiceNumberNotStartsWithINV_ErrorReturned()
    {
        // Arrange
        var invoice = GetValidInvoice();
        invoice.InvoiceNumber = "123456789";

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo(nameof(CreateInvoiceDTO.InvoiceNumber)));
        Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo(Handlers.Constants.Validation.Invoice.InvoiceNumberErrorMessage));
    }

    // IssueDate and DueDate tests
    [Test]
    public async Task ValidateAsync_IssueDateIsInFuture_ErrorReturned()
    {
        // Arrange
        var invoice = GetValidInvoice();
        invoice.IssueDate = DateTime.UtcNow.AddDays(5);

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Has.Count.EqualTo(1));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo(nameof(CreateInvoiceDTO.IssueDate)));
        Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo(Handlers.Constants.Validation.Invoice.IssueDateErrorMessage));
    }

    [Test]
    public async Task ValidateAsync_DueDateIsLessThenIssueDate_ErrorReturned()
    {
        // Arrange
        var invoice = GetValidInvoice();
        invoice.IssueDate = DateTime.UtcNow.AddDays(-5);
        invoice.DueDate = DateTime.UtcNow.AddDays(-10);

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Has.Count.EqualTo(1));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo(nameof(CreateInvoiceDTO.DueDate)));
        Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo(Handlers.Constants.Validation.Invoice.DueDateErrorMessage));
    }

    // Amount tests
    [Test]
    public async Task ValidateAsync_AmountIsNegative_ErrorReturned()
    {
        // Arrange
        var invoice = GetValidInvoice();
        invoice.Amount = -1000.00m;

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Has.Count.EqualTo(1));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo(nameof(CreateInvoiceDTO.Amount)));
    }

    // BankAccountNumber tests
    [Test]
    public async Task ValidateAsync_BankAccountNumberIsNotNumericAndNotDash_ErrorReturned()
    {
        // Arrange
        var invoice = GetValidInvoice();
        invoice.BankAccountNumber = "banknumber";

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Has.Count.EqualTo(1));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo(nameof(CreateInvoiceDTO.BankAccountNumber)));
        Assert.That(result.Errors[0].ErrorMessage, Is.EqualTo(Handlers.Constants.Validation.Invoice.BankAccountNumberErrorMessage));
    }

    [Test]
    [TestCase("2232323-322323-322323")]
    [TestCase("2332-23")]
    public async Task ValidateAsync_BankAccountNumberNotCorrectLength_ErrorReturned(string bankAccountNumber)
    {
        // Arrange
        var invoice = GetValidInvoice();
        invoice.BankAccountNumber = bankAccountNumber;

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Has.Count.EqualTo(1));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo(nameof(CreateInvoiceDTO.BankAccountNumber)));
    }

    // Currency tests
    [Test]
    public async Task ValidateAsync_CurrencyCodeNotFixedLength_ErrorReturned()
    {
        // Arrange
        var invoice = GetValidInvoice();
        invoice.Currency = "US";

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Has.Count.EqualTo(1));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo(nameof(CreateInvoiceDTO.Currency)));
    }

    [Test]
    public async Task ValidateAsync_CurrencyCodeNotUpperCase_ErrorReturned()
    {
        // Arrange
        var invoice = GetValidInvoice();
        invoice.Currency = "usd";

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Has.Count.EqualTo(1));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo(nameof(CreateInvoiceDTO.Currency)));
    }

    // PaymentStatus tests
    [Test]
    public async Task ValidateAsync_PaymentStatusIsInvalid_ErrorReturned()
    {
        // Arrange
        var invoice = GetValidInvoice();
        invoice.PaymentStatus = (PaymentStatus)100;

        // Act
        var result = await _invoiceValidator.TestValidateAsync(invoice);

        // Assert
        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors, Has.Count.EqualTo(1));
        Assert.That(result.Errors[0].PropertyName, Is.EqualTo(nameof(CreateInvoiceDTO.PaymentStatus)));
    }

    private CreateInvoiceDTO GetValidInvoice()
    {
        var clientId = Guid.Parse("BA18CC29-C7FF-48C4-9B7B-456BCEF231D0");
        var customerId = Guid.Parse("6D024627-568B-4D57-B477-2274C9D807B9");

        _clientRepositoryMock.Setup(x => x.ExistsAsync(clientId, CancellationToken.None))
            .ReturnsAsync(true);
        _customerRepositoryMock.Setup(x => x.ExistsAsync(customerId, CancellationToken.None))
            .ReturnsAsync(true);

        return Fixture.Build<CreateInvoiceDTO>()
                .With(x => x.ClientId, clientId )
                .With(x => x.CustomerId, customerId)
                .With(x => x.InvoiceNumber, "INV123456")
                .With(x => x.IssueDate, DateTime.UtcNow.AddDays(-5))
                .With(x => x.DueDate, DateTime.UtcNow.AddDays(30))
                .With(x => x.Amount, 1000.00m)
                .With(x => x.PaymentStatus, PaymentStatus.Paid)
                .With(x => x.Currency, "USD")
                .With(x => x.BankAccountNumber, "1234-1234-1234")
                .With(x => x.ItemIds, new List<Guid> { Guid.NewGuid(), Guid.NewGuid() })
                .Create();
    }
}
