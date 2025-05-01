﻿using Exadel.ReportHub.Data.Models;

namespace Exadel.ReportHub.RA.Abstract;

public interface IExchangeRateRepository
{
    Task AddManyAsync(IEnumerable<ExchangeRate> exchangeRates, CancellationToken cancellationToken);

    Task<ExchangeRate> GetByCurrencyAsync(string currency, DateTime date, CancellationToken cancellationToken);
}
