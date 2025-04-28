﻿using AutoMapper;
using ErrorOr;
using Exadel.ReportHub.RA.Abstract;
using Exadel.ReportHub.SDK.DTOs.Customer;
using MediatR;

namespace Exadel.ReportHub.Handlers.Customer.Get;

public record GetCustomersRequest(Guid ClientId) : IRequest<ErrorOr<IList<CustomerDTO>>>;

public class GetCustomersHandler(ICustomerRepository customerRepository, IClientRepository clientRepository, IMapper mapper) : IRequestHandler<GetCustomersRequest, ErrorOr<IList<CustomerDTO>>>
{
    public async Task<ErrorOr<IList<CustomerDTO>>> Handle(GetCustomersRequest request, CancellationToken cancellationToken)
    {
        var isClientExists = await clientRepository.ExistsAsync(request.ClientId, cancellationToken);
        if (!isClientExists)
        {
            return Error.NotFound();
        }

        var customers = await customerRepository.GetByClientIdAsync(request.ClientId, cancellationToken);

        var customersDto = mapper.Map<List<CustomerDTO>>(customers);
        return customersDto;
    }
}
