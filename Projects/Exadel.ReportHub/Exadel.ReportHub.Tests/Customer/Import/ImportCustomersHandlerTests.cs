using System.Text;
using AutoFixture;
using AutoMapper;
using Exadel.ReportHub.Excel.Abstract;
using Exadel.ReportHub.Handlers.Customer.Import;
using Exadel.ReportHub.Handlers.Managers.Customer;
using Exadel.ReportHub.SDK.DTOs.Customer;
using Exadel.ReportHub.SDK.DTOs.Import;
using Exadel.ReportHub.Tests.Abstracts;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Exadel.ReportHub.Tests.Customer.Import;

[TestFixture]
public class ImportCustomersHandlerTests : BaseTestFixture
{
    private Mock<IExcelImporter> _excelImporter;
    private Mock<ICustomerManager> _customerManagerMock;
    private Mock<IValidator<ImportCustomerDTO>> _validatorMock;
    private Mock<IMapper> _mapperMock;

    private ImportCustomersHandler _handler;

    [SetUp]
    public void Setup()
    {
        _excelImporter = new Mock<IExcelImporter>();
        _customerManagerMock = new Mock<ICustomerManager>();
        _validatorMock = new Mock<IValidator<ImportCustomerDTO>>();
        _mapperMock = new Mock<IMapper>();
        _handler = new ImportCustomersHandler(
            _excelImporter.Object,
            _customerManagerMock.Object,
            _validatorMock.Object,
            _mapperMock.Object);
    }

    [Test]
    public async Task ImportCustomers_ValidRequest_ReturnsCreated()
    {
        // Arrange
        var importCustomerDtos = Fixture.Build<ImportCustomerDTO>().CreateMany(2).ToList();
        var clientId = Guid.NewGuid();
        var createCustomerDtos = Mapper.Map<List<CreateCustomerDTO>>(importCustomerDtos);
        foreach (var dto in createCustomerDtos)
        {
            dto.ClientId = clientId;
        }

        _mapperMock
            .Setup(x => x.Map<List<CreateCustomerDTO>>(importCustomerDtos))
            .Returns(createCustomerDtos);

        var customerDtos = Fixture.Build<CustomerDTO>().CreateMany(2).ToList();

        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes("Excel content"));

        _excelImporter
            .Setup(x => x.Read<ImportCustomerDTO>(It.Is<Stream>(str => str.Length == memoryStream.Length)))
            .Returns(importCustomerDtos);

        _customerManagerMock
            .Setup(x => x.GenerateCustomersAsync(createCustomerDtos, CancellationToken.None))
            .ReturnsAsync(customerDtos);

        _validatorMock
            .Setup(x => x.ValidateAsync(
                importCustomerDtos[0],
                CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        _validatorMock
            .Setup(x => x.ValidateAsync(
                importCustomerDtos[1],
                CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        var importDto = new ImportDTO
        {
            File = new FormFile(memoryStream, 0, memoryStream.Length, "formFile", "customers.xlsx")
        };

        // Act
        var request = new ImportCustomersRequest(clientId, importDto);
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result.IsError, Is.False);
        Assert.That(result.Value.ImportedCount, Is.EqualTo(2));
    }
}
