namespace Exadel.ReportHub.Host;

public static class Constants
{
    public static class Authorization
    {
        public const string ScopeName = "report_hub_api";
        public const string ScopeDescription = "Full access to Report Hub API";
        public const string ResourceName = "report_hub_api";

        public static class Policy
        {
            public const string SuperAdmin = "SuperAdmin";
            public const string Owner = "Owner";
            public const string ClientAdmin = "ClientAdmin";
            public const string Operator = "Operator";
            public const string AllUsers = "AllUsers";
        }
    }

    public static class Client
    {
        public static readonly Guid GlobalId = new Guid("e47501a8-547b-4dc4-ba97-e65ccfc39477");
    }

    public static class FeedPath
    {
        public static class ReportHub
        {
            public static readonly Uri Ping = new Uri("/api/ping", UriKind.Relative);
        }
    }

    public static class HttpClient
    {
        public const string PingClient = "PingClinet";
        public const string ExchangeRateClient = "ExchangeRateClient";
    }
}
