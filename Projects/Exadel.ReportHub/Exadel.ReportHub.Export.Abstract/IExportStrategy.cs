namespace Exadel.ReportHub.Export.Abstract;

public interface IExportStrategy
{
    Task<bool> SatisfyAsync(ExportFormat format, CancellationToken cancellationToken);

    Task<Stream> ExportAsync<TModel>(TModel exportModel, CancellationToken cancellationToken);
}
