using ErrorOr;
using MediatR;

namespace Exadel.ReportHub.Handlers.Features.TestFeatures.Commands.UpdateTest;

public record UpdateRequest(Guid id, string name, bool getError) : IRequest<ErrorOr<Unit>>;
