namespace Exadel.ReportHub.Host.Services;

public class PingService(HttpClient httpClient, ILogger<PingService> logger)
{
    public async Task PingAsync()
    {
        try
        {
            var response = await httpClient.GetAsync("/");

            logger.LogInformation("Ping Job:", httpClient.BaseAddress, response.StatusCode);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "PingService failed");
        }
    }
}
