using Aspose.Cells;
using Exadel.ReportHub.Excel.Abstract;

namespace Exadel.ReportHub.Excel;

public class ExcelImporter : IExcelImporter
{
    public IList<TDto> ReadFromExcel<TDto>(Stream excelStream, Func<Row, TDto> mapFunc)
        where TDto : new()
    {
        var items = new List<TDto>();

        using (var workbook = new Workbook(excelStream))
        {
            var worksheet = workbook.Worksheets[0];
            var cells = worksheet.Cells;

            for (int i = 1; i <= cells.MaxDataRow; i++)
            {
                var row = cells.Rows[i];
                var item = mapFunc(row);
                items.Add(item);
            }
        }

        return items;
    }
}
