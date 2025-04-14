using System.Text;
using Exadel.ReportHub.Csv.Abstract;
using Exadel.ReportHub.Handlers.Invoice.Import;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Import;
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
    private ImportInvoicesHandler _handler;
    private Mock<IValidator<CreateInvoiceDTO>> _invoiceValidatorMock;

    [SetUp]
    public void Setup()
    {
        _invoiceRepositoryMock = new Mock<IInvoiceRepository>();
        _csvProcessorMock = new Mock<ICsvProcessor>();
        _invoiceValidatorMock = new Mock<IValidator<CreateInvoiceDTO>>();
        _handler = new ImportInvoicesHandler(
            _csvProcessorMock.Object,
            _invoiceRepositoryMock.Object,
            Mapper,
            _invoiceValidatorMock.Object);
    }

    [Test]
    public async Task ImportInvoices_ValidRequest_ReturnsCreated()
    {
        // Arrange
        var invoiceDtos = new List<CreateInvoiceDTO>
            {
                new CreateInvoiceDTO
                {
                    ClientId = Guid.NewGuid(),
                    CustomerId = Guid.NewGuid(),
                    InvoiceNumber = "INV2022190000001",
                    IssueDate = new DateTime(2023, 1, 10, 0, 0, 0, DateTimeKind.Utc),
                    DueDate = new DateTime(2024, 1, 20, 0, 0, 0, DateTimeKind.Utc),
                    Amount = 2277.89m,
                    Currency = "USD",
                    PaymentStatus = SDK.Enums.PaymentStatus.Pending,
                    BankAccountNumber = "84901234567890"
                },
                new CreateInvoiceDTO
                {
                    ClientId = Guid.NewGuid(),
                    CustomerId = Guid.NewGuid(),
                    InvoiceNumber = "INV2025120000002",
                    IssueDate = new DateTime(2022, 1, 15, 0, 0, 0, DateTimeKind.Utc),
                    DueDate = new DateTime(2023, 2, 15, 0, 0, 0, DateTimeKind.Utc),
                    Amount = 2657.24m,
                    Currency = "EUR",
                    PaymentStatus = SDK.Enums.PaymentStatus.Unpaid,
                    BankAccountNumber = "0219876543210987"
                }
            };
        var invoices = Mapper.Map<IEnumerable<Data.Models.Invoice>>(invoiceDtos);

        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes("CSV content"));

        _csvProcessorMock
            .Setup(x => x.ReadInvoices(It.Is<Stream>(str => str.Length == memoryStream.Length)))
            .Returns(invoiceDtos);

        _invoiceValidatorMock
            .Setup(x => x.ValidateAsync(
                It.Is<CreateInvoiceDTO>(
                    dto => invoiceDtos.Any(inv => inv.InvoiceNumber == dto.InvoiceNumber)),
                CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        _invoiceRepositoryMock
            .Setup(repo => repo.AddManyAsync(invoices, CancellationToken.None))
            .Returns(Task.CompletedTask);

        var importDto = new ImportDTO
        {
            FormFile = new FormFile(memoryStream, 0, memoryStream.Length, "formFile", "invoices.csv")
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
                        x.Amount == invoiceDtos[0].Amount &&
                        x.Currency == invoiceDtos[0].Currency &&
                        (int)x.PaymentStatus == (int)invoiceDtos[0].PaymentStatus &&
                        x.BankAccountNumber == invoiceDtos[0].BankAccountNumber) &&

                        inv.Any(x =>
                        x.ClientId == invoiceDtos[1].ClientId &&
                        x.CustomerId == invoiceDtos[1].CustomerId &&
                        x.InvoiceNumber == invoiceDtos[1].InvoiceNumber &&
                        x.IssueDate == invoiceDtos[1].IssueDate &&
                        x.DueDate == invoiceDtos[1].DueDate &&
                        x.Amount == invoiceDtos[1].Amount &&
                        x.Currency == invoiceDtos[1].Currency &&
                        (int)x.PaymentStatus == (int)invoiceDtos[1].PaymentStatus &&
                        x.BankAccountNumber == invoiceDtos[1].BankAccountNumber)),
                    CancellationToken.None),
                Times.Once);
    }
}
