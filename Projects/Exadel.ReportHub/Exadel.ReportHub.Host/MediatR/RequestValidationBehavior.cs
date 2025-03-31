﻿using System.Net;
using System.Net.NetworkInformation;
using Exadel.ReportHub.Handlers.Exceptions;
using FluentValidation;
using MediatR;

namespace Exadel.ReportHub.Host.Mediatr;

public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(_validators.
            Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(validationResult => validationResult.Errors)
            .Where(failures => failures != null).
            Select(validationFailure => validationFailure.ErrorMessage)
            .Distinct()
            .ToList();

        if (failures.Any())
        {
            throw new HttpStatusCodeException(HttpStatusCode.BadRequest, failures);
        }

        return await next();
    }
}
