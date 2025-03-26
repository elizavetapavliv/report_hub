using ErrorOr;
using MediatR;

namespace Exadel.ReportHub.Handlers.Features.TestFeatures.Commands.DeleteTest;

public record DeleteRequest(Guid id, bool getError) : IRequest<ErrorOr<Unit>>;
