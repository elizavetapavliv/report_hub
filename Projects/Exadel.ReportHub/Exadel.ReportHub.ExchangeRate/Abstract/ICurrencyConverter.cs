﻿namespace Exadel.ReportHub.Ecb.Abstract;

public interface ICurrencyConverter
{
    Task<decimal> ConvertAsync(decimal amount, string fromCurrency, string toCurrency, DateTime date, CancellationToken cancellationToken);
}
