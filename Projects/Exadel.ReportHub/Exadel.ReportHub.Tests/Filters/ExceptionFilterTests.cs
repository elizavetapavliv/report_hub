using Exadel.ReportHub.Common.Exceptions;
using Exadel.ReportHub.Host.Infrastructure.Filters;
using Exadel.ReportHub.Host.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;

namespace Exadel.ReportHub.Tests;

public class ExceptionFilterTests
{
    private Mock<ILogger<ExceptionFilter>> _loggerMock;
    private Mock<IHostEnvironment> _hostEnvironmentMock;
    private ExceptionFilter _exceptionFilter;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<ExceptionFilter>>();
        _hostEnvironmentMock = new Mock<IHostEnvironment>();
        _exceptionFilter = new ExceptionFilter(_loggerMock.Object, _hostEnvironmentMock.Object);
    }

    [Test]
    public void OnException_HttpStatusCodeException_GivesExpectedResult()
    {
        var exception = new HttpStatusCodeException(StatusCodes.Status400BadRequest, new List<string> { "an error occured" });
        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            new RouteData(),
            new ActionDescriptor());
        var context = new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = exception
        };
        _exceptionFilter.OnException(context);
        Assert.That(context.ExceptionHandled, Is.True);
        var result = context.Result as ObjectResult;
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));

        var errorResponse = result.Value as ErrorResponse;
        Assert.That(errorResponse, Is.Not.Null);
        CollectionAssert.Contains(errorResponse.Errors, "an error occured");
    }

    [Test]
    public void OnException_NoHttpStatusCodeException_GivesExpectedResult()
    {
        var exception = new Exception("an error occured");
        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            new RouteData(),
            new ActionDescriptor());
        var context = new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = exception
        };
        _exceptionFilter.OnException(context);
        Assert.That(context.ExceptionHandled, Is.True);
        var result = context.Result as StatusCodeResult;
        Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }

    [Test]
    public void OnException_NoHttpStatusCodeException_DevelopmentEnv_GivesExpectedResult()
    {
        _hostEnvironmentMock.Setup(x => x.EnvironmentName).Returns("Development");
        var exception = new Exception("an error occured");
        var actionContext = new ActionContext(
            new DefaultHttpContext(),
            new RouteData(),
            new ActionDescriptor());
        var context = new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = exception
        };
        _exceptionFilter.OnException(context);
        Assert.That(context.ExceptionHandled, Is.True);
        var result = context.Result as ObjectResult;
        Assert.That(result, Is.Not.Null);
        var errorResponse = result.Value as ErrorResponse;
        Assert.That(errorResponse, Is.Not.Null);
        CollectionAssert.Contains(errorResponse.Errors, "an error occured");
    }
}
