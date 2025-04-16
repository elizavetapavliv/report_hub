using System.Globalization;
using System.Xml.Linq;
using Exadel.ReportHub.Data.Models;

namespace Exadel.ReportHub.Common.Providers;

public class ExchangeRateProvider(HttpClient httpClient) : IExchangeRateProvider
{
    public async Task<IEnumerable<ExchangeRate>> GetDailyRatesAsync()
    {
        var uri = new Uri("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");
        var response = await httpClient.GetAsync(uri);
        var result = await response.Content.ReadAsStringAsync();

        var document = XDocument.Parse(result);

        var root = document.Root.GetDefaultNamespace();

        var cubeTime = document
            .Descendants(root + "Cube")
            .SingleOrDefault(x => x.Attribute("time") != null);

        var rateDate = DateTime.Parse(cubeTime.Attribute("time").Value, CultureInfo.InvariantCulture);

        var rates = cubeTime.Elements(root + "Cube")
            .Select(x => new ExchangeRate
            {
                Currency = x.Attribute("currency").Value,
                Rate = decimal.Parse(x.Attribute("rate").Value, CultureInfo.InvariantCulture),
                Date = rateDate
            })
            .ToList();

        return rates;
    }
}
