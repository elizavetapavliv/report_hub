using Exadel.ReportHub.Data.Models;
using MediatR;

namespace Exadel.ReportHub.Handlers.Invoices;

public class ImportInvoicesCommand(Stream csvStream) : IRequest<IEnumerable<Invoice>>
{
    public Stream CsvStream { get; } = csvStream;
}
