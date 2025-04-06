using Exadel.ReportHub.Data.Models;

namespace Exadel.ReportHub.Csv.Services;

public interface ICsvInvoiceService
{
    Task<IEnumerable<Invoice>> ImportInvoiceAsync(Stream csvStream);
}
