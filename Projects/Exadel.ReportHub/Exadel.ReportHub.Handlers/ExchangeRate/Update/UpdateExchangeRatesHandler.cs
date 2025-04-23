using Exadel.ReportHub.Ecb.Abstract;
using MediatR;

namespace Exadel.ReportHub.Handlers.ExchangeRate.Update;

public record UpdateExchangeRatesRequest : IRequest<Unit>;

public class UpdateExchangeRatesHandler(IExchangeRateClient exchangeRateProvider) : IRequestHandler<UpdateExchangeRatesRequest, Unit>
{
    public async Task<Unit> Handle(UpdateExchangeRatesRequest request, CancellationToken cancellationToken)
    {
        await exchangeRateProvider.GetDailyRatesAsync(cancellationToken);

        return Unit.Value;
    }
}
