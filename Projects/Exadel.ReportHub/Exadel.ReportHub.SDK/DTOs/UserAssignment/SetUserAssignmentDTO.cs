using Exadel.ReportHub.SDK.Enums;

namespace Exadel.ReportHub.SDK.DTOs.UserAssignment;

public class SetUserAssignmentDTO
{
    public Guid UserId { get; set; }

    public Guid ClientId { get; set; }

    public UserRole Role { get; set; }
}
