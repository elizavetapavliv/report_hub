﻿namespace Exadel.ReportHub.SDK.DTOs.Item;

public class CreateItemDTO
{
    public Guid ClientId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public string Currency { get; set; }
}
