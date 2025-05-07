using Aspose.Cells;
using Exadel.ReportHub.Data.Abstract;
using Exadel.ReportHub.Export.Abstract;

namespace Exadel.ReportHub.Excel;

public class ExcelExporter : IExportStrategy
{
    public Task<bool> SatisfyAsync(ExportFormat format, CancellationToken cancellationToken)
    {
        return Task.FromResult(format == ExportFormat.Excel);
    }

    public async Task<Stream> ExportAsync<TModel>(TModel exportModel, CancellationToken cancellationToken)
        where TModel : BaseReport
    {
        return await ExportAsync([exportModel], cancellationToken);
    }

    public async Task<Stream> ExportAsync<TModel>(IEnumerable<TModel> exportModels, CancellationToken cancellationToken)
        where TModel : BaseReport
    {
        var stream = new MemoryStream();

        var workbook = new Workbook();
        var worksheet = workbook.Worksheets[0];

        var dateStyle = workbook.CreateStyle();
        dateStyle.Custom = Constants.Format.Date;
        var decimalStyle = workbook.CreateStyle();
        decimalStyle.Custom = Constants.Format.Decimal;

        var cells = worksheet.Cells;
        var properties = typeof(TModel).GetProperties();
        cells[0, 0].PutValue(nameof(BaseReport.ReportDate));
        cells[0, 1].PutValue(exportModels.FirstOrDefault()?.ReportDate ?? DateTime.UtcNow);
        cells[0, 1].SetStyle(dateStyle);

        var column = 0;
        foreach (var property in properties)
        {
            if (property.Name.Equals(nameof(BaseReport.ReportDate), StringComparison.Ordinal))
            {
                continue;
            }

            cells[1, column].PutValue(property.Name);
            column++;
        }

        var row = 2;
        foreach (var model in exportModels)
        {
            column = 0;
            foreach (var property in properties)
            {
                if (property.Name.Equals(nameof(BaseReport.ReportDate), StringComparison.Ordinal))
                {
                    continue;
                }

                var value = property.GetValue(model);
                cells[row, column].PutValue(value);

                if (value is DateTime)
                {
                    cells[row, column].SetStyle(dateStyle);
                }

                if (value is decimal)
                {
                    cells[row, column].SetStyle(decimalStyle);
                }

                column++;
            }

            row++;
        }

        worksheet.AutoFitColumns();
        worksheet.AutoFitRows();

        await workbook.SaveAsync(stream, SaveFormat.Xlsx);

        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }
}
