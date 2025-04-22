using System.Text;
using AutoFixture;
using ErrorOr;
using Exadel.ReportHub.Csv.Abstract;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.Ecb;
using Exadel.ReportHub.Ecb.Abstract;
using Exadel.ReportHub.Handlers.Invoice.Import;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Invoice;
using Exadel.ReportHub.Tests.Abstracts;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Exadel.ReportHub.Tests.Invoice.Import;

public class ImportInvoicesHandlerTests : BaseTestFixture
{
    private Mock<ICsvProcessor> _csvProcessorMock;
    private Mock<IInvoiceRepository> _invoiceRepositoryMock;
    private Mock<ICustomerRepository> _customerRepositoryMock;
    private Mock<IItemRepository> _itemRepositoryMock;
    private Mock<ICurrencyConverter> _currencyConverterMock;
    private ImportInvoicesHandler _handler;
    private Mock<IValidator<CreateInvoiceDTO>> _invoiceValidatorMock;

    [SetUp]
    public void Setup()
    {
        _csvProcessorMock = new Mock<ICsvProcessor>();
        _invoiceRepositoryMock = new Mock<IInvoiceRepository>();
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _itemRepositoryMock = new Mock<IItemRepository>();
        _currencyConverterMock = new Mock<ICurrencyConverter>();
        _invoiceValidatorMock = new Mock<IValidator<CreateInvoiceDTO>>();
        _handler = new ImportInvoicesHandler(
            _csvProcessorMock.Object,
            _invoiceRepositoryMock.Object,
            _customerRepositoryMock.Object,
            _itemRepositoryMock.Object,
            _currencyConverterMock.Object,
            Mapper,
            _invoiceValidatorMock.Object);
    }

    [Test]
    public async Task ImportInvoices_ValidRequest_ReturnsCreated()
    {
        // Arrange
        var invoiceDtos = Fixture.Build<CreateInvoiceDTO>().CreateMany(2).ToList();
        var items = new Dictionary<Guid, Item>();
        var customers = new Dictionary<Guid, Customer>();
        var amounts = new List<decimal>();
        foreach (var invoiceDto in invoiceDtos)
        {
            decimal amount = 0;
            customers.Add(invoiceDto.CustomerId, Fixture.Build<Customer>().With(x => x.Id, invoiceDto.CustomerId).With(x => x.CurrencyCode, Constants.Currency.DefalutCurrencyCode).Create());
            foreach (var itemId in invoiceDto.ItemIds)
            {
                items.Add(itemId, Fixture.Build<Item>().With(x => x.Id, itemId).With(x => x.CurrencyCode, Constants.Currency.DefalutCurrencyCode).Create());
                amount += items[itemId].Price;
            }

            amounts.Add(amount);
        }

        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes("CSV content"));

        _csvProcessorMock
            .Setup(x => x.ReadInvoices(It.Is<Stream>(str => str.Length == memoryStream.Length)))
            .Returns(invoiceDtos);

        _customerRepositoryMock
            .Setup(x => x.GetByIdsAsync(customers.Keys.ToList(), CancellationToken.None))
            .ReturnsAsync(customers.Values.ToList());

        _itemRepositoryMock
            .Setup(x => x.GetByIdsAsync(items.Keys.ToList(), CancellationToken.None))
            .ReturnsAsync(items.Values.ToList());

        foreach (var invoiceDto in invoiceDtos)
        {
            var amount = items.Where(x => invoiceDto.ItemIds.Contains(x.Key)).Sum(x => x.Value.Price);
            _currencyConverterMock
                .Setup(x => x.ConvertAsync(amount, Constants.Currency.DefalutCurrencyCode, Constants.Currency.DefalutCurrencyCode, CancellationToken.None))
                .ReturnsAsync(amount);
        }

        _invoiceValidatorMock
            .Setup(x => x.ValidateAsync(
                invoiceDtos[0],
                CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        _invoiceValidatorMock
            .Setup(x => x.ValidateAsync(
                invoiceDtos[1],
                CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        var importDto = new ImportDTO
        {
            File = new FormFile(memoryStream, 0, memoryStream.Length, "formFile", "invoices.csv")
        };

        // Act
        var request = new ImportInvoicesRequest(importDto);
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.IsError, Is.False);
        Assert.That(result.Value.ImportedCount, Is.EqualTo(2));

        _invoiceRepositoryMock.Verify(
                repo => repo.AddManyAsync(
                    It.Is<IList<Data.Models.Invoice>>(
                        inv => inv.Count() == 2 &&
                        inv.Any(x =>
                        x.ClientId == invoiceDtos[0].ClientId &&
                        x.CustomerId == invoiceDtos[0].CustomerId &&
                        x.InvoiceNumber == invoiceDtos[0].InvoiceNumber &&
                        x.IssueDate == invoiceDtos[0].IssueDate &&
                        x.DueDate == invoiceDtos[0].DueDate &&
                        x.Amount == amounts[0] &&
                        x.CurrencyId == customers[invoiceDtos[0].CustomerId].CurrencyId &&
                        x.Currency == customers[invoiceDtos[0].CustomerId].CurrencyCode &&
                        (int)x.PaymentStatus == (int)invoiceDtos[0].PaymentStatus &&
                        x.BankAccountNumber == invoiceDtos[0].BankAccountNumber) &&

                        inv.Any(x =>
                        x.ClientId == invoiceDtos[1].ClientId &&
                        x.CustomerId == invoiceDtos[1].CustomerId &&
                        x.InvoiceNumber == invoiceDtos[1].InvoiceNumber &&
                        x.IssueDate == invoiceDtos[1].IssueDate &&
                        x.DueDate == invoiceDtos[1].DueDate &&
                        x.Amount == amounts[1] &&
                        x.CurrencyId == customers[invoiceDtos[1].CustomerId].CurrencyId &&
                        x.Currency == customers[invoiceDtos[1].CustomerId].CurrencyCode &&
                        (int)x.PaymentStatus == (int)invoiceDtos[1].PaymentStatus &&
                        x.BankAccountNumber == invoiceDtos[1].BankAccountNumber)),
                    CancellationToken.None),
                Times.Once);
    }

    [Test]
    public async Task ImportInvoices_WhenAllInvoicesInvalid_ReturnsValidationErrors()
    {
        // Arrange
        var expectedErrors = new List<string>()
        {
            "Row 1: Bank account number must only contain digits and dashes.",
            "Row 1: Issue date cannot be in the future",
            "Row 2: Bank account number must only contain digits and dashes.",
            "Row 2: Issue date cannot be in the future"
        };

        var invoiceDtos = Fixture.Build<CreateInvoiceDTO>().CreateMany(2).ToList();

        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes("CSV content"));

        var errorsInvoice = new List<ValidationFailure>
        {
            new ValidationFailure("BankAccountNumber", "Bank account number must only contain digits and dashes."),
            new ValidationFailure("IssueDateErrorMessage", "Issue date cannot be in the future")
        };

        _invoiceValidatorMock
            .Setup(x => x.ValidateAsync(
                invoiceDtos[0],
                CancellationToken.None))
            .ReturnsAsync(new ValidationResult(errorsInvoice));

        _invoiceValidatorMock
            .Setup(x => x.ValidateAsync(
                invoiceDtos[1],
                CancellationToken.None))
            .ReturnsAsync(new ValidationResult(errorsInvoice));

        _csvProcessorMock
            .Setup(x => x.ReadInvoices(It.Is<Stream>(str => str.Length == memoryStream.Length)))
            .Returns(invoiceDtos);

        var importDto = new ImportDTO
        {
            File = new FormFile(memoryStream, 0, memoryStream.Length, "formFile", "invoices.csv")
        };

        // Act
        var request = new ImportInvoicesRequest(importDto);
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.IsError, Is.True);
        Assert.That(result.Errors, Has.Count.EqualTo(expectedErrors.Count));
        Assert.That(result.Errors.All(e => e.Type == ErrorType.Validation), Is.True);
        Assert.That(result.Errors[0].Description, Is.EqualTo(expectedErrors[0]));
        Assert.That(result.Errors[1].Description, Is.EqualTo(expectedErrors[1]));
        Assert.That(result.Errors[2].Description, Is.EqualTo(expectedErrors[2]));
        Assert.That(result.Errors[3].Description, Is.EqualTo(expectedErrors[3]));

        _invoiceRepositoryMock.Verify(
                repo => repo.AddManyAsync(
                    It.IsAny<IList<Data.Models.Invoice>>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
    }

    [Test]
    public async Task ImportInvoices_WhenTheOnlyInvoiceInvalid_ReturnsValidationErrors()
    {
        // Arrange
        var expectedErrors = new List<string>()
        {
            "Row 2: Bank account number must only contain digits and dashes.",
            "Row 2: Issue date cannot be in the future"
        };

        var invoiceDtos = Fixture.Build<CreateInvoiceDTO>().CreateMany(2).ToList();

        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes("CSV content"));

        var errorsInvoice = new List<ValidationFailure>
        {
            new ValidationFailure("BankAccountNumber", "Bank account number must only contain digits and dashes."),
            new ValidationFailure("IssueDateErrorMessage", "Issue date cannot be in the future")
        };

        _invoiceValidatorMock
            .Setup(x => x.ValidateAsync(
                invoiceDtos[0],
                CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        _invoiceValidatorMock
            .Setup(x => x.ValidateAsync(
                invoiceDtos[1],
                CancellationToken.None))
            .ReturnsAsync(new ValidationResult(errorsInvoice));

        _csvProcessorMock
            .Setup(x => x.ReadInvoices(It.Is<Stream>(str => str.Length == memoryStream.Length)))
            .Returns(invoiceDtos);

        var importDto = new ImportDTO
        {
            File = new FormFile(memoryStream, 0, memoryStream.Length, "formFile", "invoices.csv")
        };

        // Act
        var request = new ImportInvoicesRequest(importDto);
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.IsError, Is.True);
        Assert.That(result.Errors, Has.Count.EqualTo(expectedErrors.Count));
        Assert.That(result.Errors.All(e => e.Type == ErrorType.Validation), Is.True);
        Assert.That(result.Errors[0].Description, Is.EqualTo(expectedErrors[0]));
        Assert.That(result.Errors[1].Description, Is.EqualTo(expectedErrors[1]));

        _invoiceRepositoryMock.Verify(
                repo => repo.AddManyAsync(
                    It.IsAny<IList<Data.Models.Invoice>>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
    }
}
