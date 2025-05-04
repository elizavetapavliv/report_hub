namespace Exadel.ReportHub.Export.Abstract;

public interface IExportStrategy
{
    Task<Stream> GenerateAsync<TModel>(TModel exportModel, CancellationToken cancellationToken);
}
