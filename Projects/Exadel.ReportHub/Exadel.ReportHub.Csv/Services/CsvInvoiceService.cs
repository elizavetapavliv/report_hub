using Exadel.ReportHub.Data.Models;

namespace Exadel.ReportHub.Csv.Services;

public class CsvInvoiceService(CsvProcessor _csvProcessor) : ICsvInvoiceService
{
    public Task<IEnumerable<Invoice>> ImportInvoiceAsync(Stream csvStream)
    {
        var invoices = _csvProcessor.ParseInvoice(csvStream);
        return Task.FromResult(invoices);
    }
}

