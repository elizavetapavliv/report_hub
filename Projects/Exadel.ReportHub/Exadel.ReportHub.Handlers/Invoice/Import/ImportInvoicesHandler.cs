using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.Csv.Abstracts;
using Exadel.ReportHub.RA.Abstract;
using MediatR;

namespace Exadel.ReportHub.Handlers.Invoice.Import;

public record ImportInvoicesRequest(Stream CsvStream, string FileName) : IRequest<ErrorOr<Created>>;
public class ImportInvoicesHandler(ICsvProcessor csvProcessor, IInvoiceRepository invoiceRepository, IMapper mapper) : IRequestHandler<ImportInvoicesRequest, ErrorOr<Created>>
{
    public async Task<ErrorOr<Created>> Handle(ImportInvoicesRequest request, CancellationToken cancellationToken)
    {
        var invoices = mapper.Map<IEnumerable<Data.Models.Invoice>>(csvProcessor.ReadInvoices(request.CsvStream));
        await invoiceRepository.ImportAsync(invoices, cancellationToken);

        return Result.Created;
    }
}
