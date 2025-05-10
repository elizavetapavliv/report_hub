using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Aspose.Cells;
using Exadel.ReportHub.Excel.Abstract;

namespace Exadel.ReportHub.Excel;

public class ExcelImporter : IExcelImporter
{
    public IList<TDto> Read<TDto>(Stream excelStream)
         where TDto : new()
    {
        using var workbook = new Workbook(excelStream);
        using var worksheet = workbook.Worksheets[0];
        var cells = worksheet.Cells;

        var header = GetHeader(cells);
        var properties = typeof(TDto).GetProperties();

        return ExtractRows<TDto>(cells, header, properties);
    }

    private IDictionary<string, int> GetHeader(Cells cells)
    {
        var headerMap = new Dictionary<string, int>();
        for (int column = 0; column <= cells.MaxDataColumn; column++)
        {
            var header = cells[0, column].StringValue?.Trim();
            if (!string.IsNullOrWhiteSpace(header) && !headerMap.ContainsKey(header))
            {
                headerMap[header] = column;
            }
        }

        return headerMap;
    }

    private IList<TDto> ExtractRows<TDto>(Cells cells, IDictionary<string, int> headerMap, PropertyInfo[] properties)
        where TDto : new()
    {
        var items = new List<TDto>();

        for (int row = 1; row <= cells.MaxDataRow; row++)
        {
            var dto = new TDto();
            PopulateDto(cells, row, dto, headerMap, properties);
            items.Add(dto);
        }

        return items;
    }

    private void PopulateDto<TDto>(Cells cells, int row, TDto dto, IDictionary<string, int> headerMap, PropertyInfo[] properties)
    {
        foreach (var property in properties)
        {
            if (!headerMap.ContainsKey(property.Name))
            {
                continue;
            }

            var columnIndex = headerMap[property.Name];
            var cell = cells[row, columnIndex];
            if (cell?.Value == null)
            {
                continue;
            }

            var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

            if (targetType == typeof(Guid) || targetType == typeof(Guid?))
            {
                HandleGuidConversion(cell, dto, property, targetType);
            }
            else
            {
                var convertedValue = Convert.ChangeType(cell.Value, targetType);
                property.SetValue(dto, convertedValue);
            }
        }
    }

    private void HandleGuidConversion<TDto>(Cell cell, TDto dto, PropertyInfo property, Type targetType)
    {
        if (cell.Value is string stringValue && Guid.TryParse(stringValue, out var guidValue))
        {
            property.SetValue(dto, guidValue);
        }
        else
        {
            property.SetValue(dto, targetType == typeof(Guid?) ? (Guid?)null : Guid.Empty);
        }
    }
}
