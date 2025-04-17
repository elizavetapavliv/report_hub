using System.Globalization;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Exadel.ReportHub.ExchangeRate;

public class ExchangeRateProvider(HttpClient httpClient, IOptions<ExchangeRateConfig> config, ILogger<ExchangeRateProvider> logger) : IExchangeRateProvider
{
    public async Task<IEnumerable<Data.Models.ExchangeRate>> GetDailyRatesAsync()
    {
        var response = await httpClient.GetAsync(config.Value.FeedUri);
        var result = await response.Content.ReadAsStringAsync();

        XDocument document;
        try
        {
            document = XDocument.Parse(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to parse ECB xml");
            return new List<Data.Models.ExchangeRate>();
        }

        var root = document.Root.GetDefaultNamespace();

        var cubeTime = document
            .Descendants(root + "Cube")
            .SingleOrDefault(x => x.Attribute("time") != null);

        if(cubeTime is null)
        {
            logger.LogError("ECb didn't contain Cube time element");
            return new List<Data.Models.ExchangeRate>();
        }

        var rateDate = DateTime.Parse(cubeTime.Attribute("time").Value, CultureInfo.InvariantCulture);

        var rates = cubeTime.Elements(root + "Cube")
            .Select(x => new Data.Models.ExchangeRate
            {
                Currency = x.Attribute("currency").Value,
                Rate = decimal.Parse(x.Attribute("rate").Value, CultureInfo.InvariantCulture),
                Date = rateDate
            })
            .ToList();

        return rates;
    }
}
