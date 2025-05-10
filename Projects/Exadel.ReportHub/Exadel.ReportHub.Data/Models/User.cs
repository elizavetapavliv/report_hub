using Exadel.ReportHub.Data.Abstract;
using Exadel.ReportHub.Data.Enums;

namespace Exadel.ReportHub.Data.Models;

public class User : IDocument, IActivable
{
    public Guid Id { get; set; }

    public string Email { get; set; }

    public string FullName { get; set; }

    public string PasswordHash { get; set; }

    public string PasswordSalt { get; set; }

    public bool IsActive { get; set; } = true;

    public NotificationSettings NotificationSettings { get; set; }
}
