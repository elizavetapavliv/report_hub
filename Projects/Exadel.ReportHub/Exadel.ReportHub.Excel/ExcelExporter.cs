using System.Reflection;
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
        var stream = new MemoryStream();

        var workbook = new Workbook();
        var worksheet = workbook.Worksheets[0];

        var cells = worksheet.Cells;
        var properties = typeof(TModel).GetProperties();

        SetHeaders(cells, properties);
        SetValues(workbook, cells, properties, exportModel);

        worksheet.AutoFitColumns();
        worksheet.AutoFitRows();

        await workbook.SaveAsync(stream, SaveFormat.Xlsx);

        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }

    public async Task<Stream> ExportAsync<TModel>(IEnumerable<TModel> exportModels, CancellationToken cancellationToken)
        where TModel : BaseReport
    {
        var stream = new MemoryStream();

        var workbook = new Workbook();
        var worksheet = workbook.Worksheets[0];

        var cells = worksheet.Cells;
        var properties = typeof(TModel).GetProperties();

        SetHeaders(cells, properties);
        SetValues(workbook, cells, properties, exportModels.ToArray());

        worksheet.AutoFitColumns();
        worksheet.AutoFitRows();

        await workbook.SaveAsync(stream, SaveFormat.Xlsx);

        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }

    private void SetHeaders(Cells cells, PropertyInfo[] properties)
    {
        for (int i = 0; i < properties.Length; i++)
        {
            cells[0, i].PutValue(properties[i].Name);
        }
    }

    private void SetValues<TModel>(Workbook workbook, Cells cells, PropertyInfo[] properties, params TModel[] exportModels)
    {
        var dateStyle = workbook.CreateStyle();
        dateStyle.Custom = Constants.Format.Date;
        var decimalStyle = workbook.CreateStyle();
        decimalStyle.Custom = Constants.Format.Decimal;

        for (int row = 1; row <= exportModels.Length; row++)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                var value = properties[i].GetValue(exportModels[row - 1]);
                cells[row, i].PutValue(value);

                if (value is DateTime)
                {
                    cells[row, i].SetStyle(dateStyle);
                }

                if (value is decimal)
                {
                    cells[row, i].SetStyle(decimalStyle);
                }
            }
        }
    }
}
