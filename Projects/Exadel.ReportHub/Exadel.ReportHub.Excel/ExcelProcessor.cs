using Aspose.Cells;
using Exadel.ReportHub.Excel.Abstract;
using Exadel.ReportHub.SDK.DTOs.Client;
using Exadel.ReportHub.SDK.DTOs.Customer;

namespace Exadel.ReportHub.Excel;

public class ExcelProcessor : IExcelProcessor
{
    public IList<CreateClientDTO> ReadClients(Stream excelStream)
    {
        var clients = new List<CreateClientDTO>();
        var workbook = new Workbook(excelStream);
        var worksheet = workbook.Worksheets[0];
        var cells = worksheet.Cells;

        for (int i = 1; i <= cells.MaxDataRow; i++)
        {
            var name = cells[i, 0].StringValue;
            var bankAccountNumber = cells[i, 1].StringValue;
            var countryId = Guid.Parse(cells[i, 2].StringValue);

            var dto = new CreateClientDTO
            {
                Name = name,
                BankAccountNumber = bankAccountNumber,
                CountryId = countryId,
            };

            clients.Add(dto);
        }

        return clients;
    }

    public IList<CreateCustomerDTO> ReadCustomers(Stream excelStream)
    {
        var customers = new List<CreateCustomerDTO>();
        var workbook = new Workbook(excelStream);
        var worksheet = workbook.Worksheets[0];
        var cells = worksheet.Cells;

        for (int i = 1; i <= cells.MaxDataRow; i++)
        {
            var name = cells[i, 0].StringValue;
            var countryId = Guid.Parse(cells[i, 1].StringValue);
            var email = cells[i, 2].StringValue;
            var clientId = Guid.Parse(cells[i, 3].StringValue);

            var dto = new CreateCustomerDTO
            {
                Name = name,
                Email = email,
                CountryId = countryId,
                ClientId = clientId
            };

            customers.Add(dto);
        }

        return customers;
    }
}
