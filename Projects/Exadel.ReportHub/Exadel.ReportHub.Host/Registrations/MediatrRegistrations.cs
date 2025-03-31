using Exadel.ReportHub.Handlers.Test;
using Exadel.ReportHub.Handlers.UserHandlers.Validators;
using Exadel.ReportHub.Host.Mediatr;
using FluentValidation;
using MediatR;

namespace Exadel.ReportHub.Host.Registrations;

public static class MediatrRegistrations
{
    public static void AddMediatr(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateHandler).Assembly));

        services.AddValidatorsFromAssembly(typeof(CreateUserValidator).Assembly);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
    }
}
