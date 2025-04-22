namespace Exadel.ReportHub.Handlers.Invoice.Import.CurrencyConverter_replace;

public interface ICurrencyConverter
{
    Task<decimal> ConvertAsync(decimal amount, string fromCurrency, string toCurrency, CancellationToken cancellationToken);
}
