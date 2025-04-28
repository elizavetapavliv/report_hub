using ErrorOr;
using Exadel.ReportHub.RA;
using Exadel.ReportHub.RA.Abstract;
using MediatR;

namespace Exadel.ReportHub.Handlers.Customer.Delete;

public record DeleteCustomerRequest(Guid CustomerId, Guid ClientId) : IRequest<ErrorOr<Deleted>>;

public class DeleteCustomerHandler(ICustomerRepository customerRepository) : IRequestHandler<DeleteCustomerRequest, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
    {
        var isCustomerExists = await customerRepository.ExistsAsync(request.CustomerId, cancellationToken);
        if (!isCustomerExists)
        {
            return Error.NotFound();
        }

        var isClientCorrect = request.ClientId == await customerRepository.GetClientIdAsync(request.CustomerId, cancellationToken);
        if (!isClientCorrect)
        {
            return Error.Forbidden();
        }

        await customerRepository.SoftDeleteAsync(request.CustomerId, cancellationToken);
        return Result.Deleted;
    }
}
