namespace Exadel.ReportHub.Host.Configs;

public class ReportHubConfig
{
    public Uri Host { get; init; }

    public TimeSpan TimeSpan { get; set; } = TimeSpan.FromSeconds(10);
}
