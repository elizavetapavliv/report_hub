using System.Text;
using AutoFixture;
using ErrorOr;
using Exadel.ReportHub.Csv.Abstracts;
using Exadel.ReportHub.Handlers.Invoice.Import;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Invoice;
using Exadel.ReportHub.SDK.DTOs.User;
using Exadel.ReportHub.Tests.Abstracts;
using Moq;

namespace Exadel.ReportHub.Tests.Invoice.Import;

public class ImportInvoicesHandlerTests : BaseTestFixture
{
    private Mock<ICsvProcessor> _csvProcessorMock;
    private Mock<IInvoiceRepository> _invoiceRepositoryMock;
    private ImportInvoicesHandler _handler;

    [SetUp]
    public void Setup()
    {
        _invoiceRepositoryMock = new Mock<IInvoiceRepository>();
        _csvProcessorMock = new Mock<ICsvProcessor>();
        _handler = new ImportInvoicesHandler(_csvProcessorMock.Object, _invoiceRepositoryMock.Object, Mapper);
    }

    [Test]
    public async Task ImportInvoices_ValidRequest_ReturnsCreated()
    {
        // Arrange
        var invoiceDtos = Fixture.CreateMany<CreateInvoiceDTO>(5);
        var invoices = Mapper.Map<IEnumerable<Data.Models.Invoice>>(invoiceDtos);

        _csvProcessorMock
            .Setup(x => x.ReadInvoices(It.IsAny<Stream>()))
            .Returns(invoiceDtos);

        _invoiceRepositoryMock
            .Setup(repo => repo.ImportAsync(invoices, CancellationToken.None))
            .Returns(Task.CompletedTask);

        // Act
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("Test Stream"));
        var request = new ImportInvoicesRequest(stream, "file.csv");
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.IsError, Is.False);

        _invoiceRepositoryMock.Verify(
            mock => mock.ImportAsync(
                It.Is<IEnumerable<Data.Models.Invoice>>(
                    i => i.Count() == invoiceDtos.Count()),
                CancellationToken.None),
            Times.Once);
    }
}
