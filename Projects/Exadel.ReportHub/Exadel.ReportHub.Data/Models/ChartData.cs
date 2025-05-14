namespace Exadel.ReportHub.Data.Models;

public class ChartData
{
    public string ChartTitle { get; set; }

    public string NamesTitle { get; set; }

    public string ValuesTitle { get; set; }

    public Dictionary<string, decimal> NameValues { get; set; }
}
