using Exadel.ReportHub.SDK.DTOs.Client;
using Exadel.ReportHub.SDK.DTOs.Customer;

namespace Exadel.ReportHub.Excel.Abstract;

public interface IExcelProcessor
{
    IList<CreateClientDTO> ReadClients(Stream excelStream);

    IList<CreateCustomerDTO> ReadCustomers(Stream excelStream);
}
