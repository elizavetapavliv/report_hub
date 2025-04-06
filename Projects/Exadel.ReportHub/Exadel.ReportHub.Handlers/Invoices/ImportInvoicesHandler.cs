using Exadel.ReportHub.Csv.Services;
using Exadel.ReportHub.Data.Models;
using MediatR;
using InvoiceModel = Exadel.ReportHub.Data.Models.Invoice;

namespace Exadel.ReportHub.Handlers.Invoices;

public class ImportInvoicesHandler(ICsvInvoiceService csvInvoiceService) : IRequestHandler<ImportInvoicesCommand, IEnumerable<Invoice>>
{
    public async Task<IEnumerable<InvoiceModel>> Handle(ImportInvoicesCommand request, CancellationToken cancellationToken)
    {
        return await csvInvoiceService.ImportInvoiceAsync(request.CsvStream);
    }
}
