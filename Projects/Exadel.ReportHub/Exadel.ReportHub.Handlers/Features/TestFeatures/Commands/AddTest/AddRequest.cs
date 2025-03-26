using ErrorOr;
using MediatR;

namespace Exadel.ReportHub.Handlers.Features.TestFeatures.Commands.AddTest;

public record AddRequest(string name, bool getError) : IRequest<ErrorOr<Unit>>;
