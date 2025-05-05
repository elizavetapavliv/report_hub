namespace Exadel.ReportHub.SDK.DTOs.Pagination;

public class PageResultDTO<T>
    where T : class
{
    public long TotalCount { get; set; }

    public int PageSize { get; set; }

    public IList<T> Entities { get; set; }
}
