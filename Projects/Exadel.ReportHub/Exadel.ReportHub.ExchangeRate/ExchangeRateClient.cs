using System.Globalization;
using System.Xml.Linq;
using Exadel.ReportHub.Data.Models;
using Exadel.ReportHub.Ecb.Abstract;
using Microsoft.Extensions.Logging;

namespace Exadel.ReportHub.Ecb;

public class ExchangeRateClient(IHttpClientFactory factory, ILogger<ExchangeRateClient> logger) : IExchangeRateClient
{
    public async Task<IList<ExchangeRate>> GetWeekByCurrencyAsync(string currency, DateTime date, CancellationToken cancellationToken)
    {
        var client = factory.CreateClient(Constants.ClientName);
        var daysCount = 7;

        HttpResponseMessage response;
        try
        {
            response = await client.GetAsync(new Uri(string.Format(Constants.Path.ExchangeRatePathTemplate, currency,
                DateOnly.FromDateTime(date.AddDays(-daysCount)).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                DateOnly.FromDateTime(date).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)), UriKind.Relative), cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, Constants.Error.HttpFetchError);
            return new List<ExchangeRate>();
        }
        catch (TaskCanceledException ex)
        {
            logger.LogError(ex, Constants.Error.TimeoutError);
            return new List<ExchangeRate>();
        }

        var result = await response.Content.ReadAsStringAsync(cancellationToken);

        XDocument document;
        try
        {
            document = XDocument.Parse(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, Constants.Error.ParseError);
            return new List<ExchangeRate>();
        }

        var generic = document.Root.GetNamespaceOfPrefix("generic");
        var series = document.Descendants(generic + "Series").SingleOrDefault();

        var rates = series.Elements(generic + "Obs")
            .Select(x => new ExchangeRate
            {
                Id = Guid.NewGuid(),
                Currency = currency,
                Rate = decimal.Parse(x.Element(generic + "ObsValue").Attribute("value").Value, CultureInfo.InvariantCulture),
                RateDate = DateTime.Parse(x.Element(generic + "ObsDimension").Attribute("value").Value,
                    CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)
            })
            .ToList();

        return rates;
    }

    public async Task<ExchangeRate> GetByCurrencyAsync(string currency, DateTime date, CancellationToken cancellationToken)
    {
        return (await GetWeekByCurrencyAsync(currency, date, cancellationToken)).OrderByDescending(x => x.RateDate).FirstOrDefault();
    }
}
