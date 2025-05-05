using Exadel.ReportHub.Export.Abstract.Models;

namespace Exadel.ReportHub.Export.Abstract;

public interface IExportStrategy
{
    Task<Stream> ExportAsync<TModel>(TModel exportModel, CancellationToken cancellationToken)
        where TModel : BaseReport;

    bool Satisfy(ExportFormat format);
}
