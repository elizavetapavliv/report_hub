using Aspose.Cells;

namespace Exadel.ReportHub.Excel.Abstract;

public interface IExcelImporter
{
    IList<TDto> ReadFromExcel<TDto>(Stream excelStream, Func<Row, TDto> mapFunc)
        where TDto : new();
}
