namespace Exadel.ReportHub.SDK.DTOs.Item;

public class UpdateItemDTO
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public Guid CurrencyId { get; set; }
}
