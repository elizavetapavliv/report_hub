using Aspose.Cells;

namespace Exadel.ReportHub.Excel.Abstract;

public interface IExcelImporter
{
    IEnumerable<TDto> Read<TDto>(Stream excelStream)
        where TDto : new();
}
