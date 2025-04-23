﻿namespace Exadel.ReportHub.Ecb;

public static class Constants
{
    public const string ClientName = "ExchangeRateClient";

    public static class Currency
    {
        public const string DefalutCurrencyCode = "EUR";
    }

    public static class Error
    {
        public const string HttpFetchError = "HTTP error fetching ECB rates";
        public const string TimeoutError = "Timeout fetching ECB rates";
        public const string ParseError = "Failed to parse ECB xml";

        public static class ExchangeRate
        {
            public const string RatesUpdateError = "Rates update Error";
            public const string EcbReturnsNothing = "ECB returns nothing";
        }
    }

    public static class Path
    {
        public static readonly Uri ExchangeRate = new Uri("/stats/eurofxref/eurofxref-daily.xml", UriKind.Relative);
    }
}
