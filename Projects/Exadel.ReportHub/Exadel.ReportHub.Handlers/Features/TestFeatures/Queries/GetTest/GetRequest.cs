using ErrorOr;
using MediatR;

namespace Exadel.ReportHub.Handlers.Features.TestFeatures.Queries.GetTest;

public record GetRequest(bool getError) : IRequest<ErrorOr<string>>;
