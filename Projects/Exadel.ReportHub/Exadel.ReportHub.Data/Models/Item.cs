using Exadel.ReportHub.Data.Abstract;

namespace Exadel.ReportHub.Data.Models;

public class Item : IDocument, ISoftDeletable
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public Guid CurrencyId { get; set; }

    public string CurrencyCode { get; set; }

    public bool IsDeleted { get; set; }
}
