using System.Reflection;
using Exadel.ReportHub.Handlers.Test;
using Exadel.ReportHub.Host.Mediatr;
using FluentValidation;
using MediatR;

namespace Exadel.ReportHub.Host.Registrations;

public static class MediatrRegistrations
{
    public static void AddMediatr(this IServiceCollection services)
    {
        var assembly = typeof(CreateHandler).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidator(assembly);
    }

    private static void AddValidator(this IServiceCollection services, Assembly assembly)
    {
        services.AddValidatorsFromAssembly(assembly);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
    }
}
