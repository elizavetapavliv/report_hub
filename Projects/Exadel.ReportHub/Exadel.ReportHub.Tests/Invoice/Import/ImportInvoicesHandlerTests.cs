using System.Text;
using Exadel.ReportHub.Csv.Abstract;
using Exadel.ReportHub.Handlers.Invoice.Import;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Invoice;
using Exadel.ReportHub.SDK.Models;
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
        _handler = new ImportInvoicesHandler(_csvProcessorMock.Object, _invoiceRepositoryMock.Object, Mapper, _invoiceValidatorMock.Object);
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
                    BankAccountNumber = "849012345678901234567890"
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
                    BankAccountNumber = "021987654321098765432109"
                }
            };
        var invoices = Mapper.Map<IEnumerable<Data.Models.Invoice>>(invoiceDtos);

        _csvProcessorMock
            .Setup(x => x.ReadInvoices(It.IsAny<Stream>()))
            .Returns(invoiceDtos);

        _invoiceValidatorMock
            .Setup(x => x.ValidateAsync(It.IsAny<CreateInvoiceDTO>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        _invoiceRepositoryMock
            .Setup(repo => repo.AddManyAsync(invoices, CancellationToken.None))
            .Returns(Task.CompletedTask);

        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes("Fake CSV content"));
        var fileModel = new FileModel
        {
            FormFile = new FormFile(memoryStream, 0, memoryStream.Length, "formFile", "invoices.csv")
        };

        // Act
        var request = new ImportInvoicesRequest(fileModel);
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.IsError, Is.False);
        Assert.That(result.Value, Is.EqualTo("2 invoices imported"));

        _invoiceRepositoryMock.Verify(
                repo => repo.AddManyAsync(
                    It.Is<IEnumerable<Data.Models.Invoice>>(
                        inv => inv.Count() == 2),
                    CancellationToken.None),
                Times.Once);
    }
}
