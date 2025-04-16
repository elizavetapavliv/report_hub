using ErrorOr;
using Exadel.ReportHub.Common.Providers;
using Exadel.ReportHub.RA.Abstract;
using MediatR;

namespace Exadel.ReportHub.Handlers.ExchangeRate.Update;

public record UpdateExchangeRatesRequest : IRequest<ErrorOr<Updated>>;
public class UpdateExchangeRatesHandler(IExchangeRateProvider exchangeRateProvider, IExchangeRepository exchangeRepository) : IRequestHandler<UpdateExchangeRatesRequest, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateExchangeRatesRequest request, CancellationToken cancellationToken)
    {
        var rates = await exchangeRateProvider.GetDailyRatesAsync();
        if(rates is null)
        {
            return Error.Failure("ECB_ERROR");
        }

        await exchangeRepository.AddManyAsync(rates, cancellationToken);

        return Result.Updated;
    }
}
