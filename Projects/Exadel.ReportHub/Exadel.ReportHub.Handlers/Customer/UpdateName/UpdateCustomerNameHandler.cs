using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using MediatR;

namespace Exadel.ReportHub.Handlers.Customer.UpdateName;

public record UpdateCustomerNameRequest(Guid Id, string Name) : IRequest<ErrorOr<Updated>>;

public class UpdateCustomerNameHandler(ICustomerRepository customerRepository) : IRequestHandler<UpdateCustomerNameRequest, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateCustomerNameRequest request, CancellationToken cancellationToken)
    {
        var isExists = await customerRepository.ExistsAsync(request.Id, cancellationToken);
        if (!isExists)
        {
            return Error.NotFound();
        }

        await customerRepository.UpdateNameAsync(request.Id, request.Name, cancellationToken);
        return Result.Updated;
    }
}
