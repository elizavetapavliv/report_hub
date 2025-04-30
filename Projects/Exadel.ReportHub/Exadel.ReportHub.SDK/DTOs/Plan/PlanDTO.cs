using Exadel.ReportHub.SDK.DTOs.Date;

namespace Exadel.ReportHub.SDK.DTOs.Plan;

public class PlanDTO : DatesDTO
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }

    public Guid ItemId { get; set; }

    public int Amount { get; set; }
}
