﻿namespace Exadel.ReportHub.SDK.DTOs.Plan;

public class PlanDTO
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }

    public Guid ItemId { get; set; }

    public int Amount { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}
