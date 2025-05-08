using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.Handlers.Managers.Common;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Customer;
using MediatR;

namespace Exadel.ReportHub.Handlers.Customer.Create;

public record CreateCustomerRequest(CreateCustomerDTO CreateCustomerDTO) : IRequest<ErrorOr<CustomerDTO>>;

public class CreateCustomerHandler(
    ICustomerRepository customerRepository,
    IMapper mapper,
    ICountryBasedEntityManager<CreateCustomerDTO, Data.Models.Customer> countryBasedEntityManager)
    : IRequestHandler<CreateCustomerRequest, ErrorOr<CustomerDTO>>
{
    public async Task<ErrorOr<CustomerDTO>> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await countryBasedEntityManager.GenerateEntityAsync(request.CreateCustomerDTO, cancellationToken);

        await customerRepository.AddAsync(customer, cancellationToken);
        return mapper.Map<CustomerDTO>(customer);
    }
}
