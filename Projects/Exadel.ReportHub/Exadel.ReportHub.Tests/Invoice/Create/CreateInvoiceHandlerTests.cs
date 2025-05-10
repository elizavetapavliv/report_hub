using AutoFixture;
using Exadel.ReportHub.Handlers.Invoice.Create;
using Exadel.ReportHub.Handlers.Managers;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Invoice;
using Exadel.ReportHub.Tests.Abstracts;
using Moq;

namespace Exadel.ReportHub.Tests.Invoice.Create;

[TestFixture]
public class CreateInvoiceHandlerTests : BaseTestFixture
{
    private Mock<IInvoiceRepository> _invoiceRepositoryMock;
    private Mock<IInvoiceManager> _invoiceManagerMock;
    private CreateInvoiceHandler _handler;

    [SetUp]
    public void Setup()
    {
        _invoiceRepositoryMock = new Mock<IInvoiceRepository>();
        _invoiceManagerMock = new Mock<IInvoiceManager>();
        _handler = new CreateInvoiceHandler(_invoiceRepositoryMock.Object, _invoiceManagerMock.Object, Mapper);
    }

    [Test]
    public async Task CreateInvoice_ValidRequest_ReturnsInvoiceDto()
    {
        // Arrange
        var createInvoiceDto = Fixture.Create<CreateInvoiceDTO>();
        var generatedInvoice = Fixture
                .Build<Data.Models.Invoice>()
                .With(x => x.Id, Guid.NewGuid())
                .With(x => x.ClientId, createInvoiceDto.ClientId)
                .With(x => x.CustomerId, createInvoiceDto.CustomerId)
                .With(x => x.InvoiceNumber, createInvoiceDto.InvoiceNumber)
                .With(x => x.IssueDate, createInvoiceDto.IssueDate)
                .With(x => x.DueDate, createInvoiceDto.DueDate)
                .With(x => x.BankAccountNumber, createInvoiceDto.BankAccountNumber)
                .With(x => x.PaymentStatus, (Data.Enums.PaymentStatus)createInvoiceDto.PaymentStatus)
                .With(x => x.ItemIds, createInvoiceDto.ItemIds.ToList())
                .Create();

        _invoiceManagerMock
                .Setup(m => m.GenerateInvoiceAsync(createInvoiceDto, It.IsAny<CancellationToken>()))
                .ReturnsAsync(generatedInvoice);

        // Act
        var result = await _handler.Handle(new CreateInvoiceRequest(createInvoiceDto), CancellationToken.None);

        // Assert
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.IsError, Is.False);
        Assert.That(result.Value, Is.InstanceOf<InvoiceDTO>(), "Returned object should be an instance of InvoiceDTO");
        Assert.That(result.Value.Id, Is.Not.EqualTo(Guid.Empty));

        _invoiceRepositoryMock.Verify(
            mock => mock.AddAsync(
                It.Is<Data.Models.Invoice>(
                    i => i.ClientId == createInvoiceDto.ClientId &&
                    i.CustomerId == createInvoiceDto.CustomerId &&
                    i.InvoiceNumber == createInvoiceDto.InvoiceNumber &&
                    i.IssueDate == createInvoiceDto.IssueDate &&
                    i.DueDate == createInvoiceDto.DueDate &&
                    i.PaymentStatus == (Data.Enums.PaymentStatus)createInvoiceDto.PaymentStatus &&
                    i.BankAccountNumber == createInvoiceDto.BankAccountNumber &&
                    i.ItemIds.SequenceEqual(createInvoiceDto.ItemIds)),
                CancellationToken.None),
            Times.Once);
    }
}
