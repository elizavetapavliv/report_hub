using Aspose.Cells;
using Exadel.ReportHub.Export.Abstract;
using Exadel.ReportHub.Export.Abstract.Models;


namespace Exadel.ReportHub.Excel;

public class ExcelProcessor : IExportStrategy
{
    public async Task<Stream> GenerateAsync<TModel>(TModel exportModel, CancellationToken cancellationToken)
        where TModel : BaseReportModel
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

        for (int i = 0; i < properties.Length; i++)
        {
            var value = properties[i].GetValue(exportModel);

            cells[0, i].PutValue(properties[i].Name);
            cells[1, i].PutValue(value);

            if (value is DateTime)
            {
                cells[1, i].SetStyle(dateStyle);
            }
            
            if (value is decimal)
            {
                cells[1, i].SetStyle(decimalStyle);
            }
        }

        worksheet.AutoFitColumns();
        worksheet.AutoFitRows();

        await workbook.SaveAsync(stream, SaveFormat.Xlsx);

        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }
}
