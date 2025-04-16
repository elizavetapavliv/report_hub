﻿using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Handlers.Customer.Create;
using Exadel.ReportHub.Handlers.Customer.Delete;
using Exadel.ReportHub.Handlers.Customer.Get;
using Exadel.ReportHub.Handlers.Customer.GetById;
using Exadel.ReportHub.Handlers.Customer.Update;
using Exadel.ReportHub.SDK.DTOs.Customer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.ReportHub.Host.Services;

[ExcludeFromCodeCoverage]
[Route("api/customers")]
[ApiController]
public class CustomersService(ISender sender) : BaseService
{
    [Authorize(Policy = Constants.Authorization.Policy.Owner)]
    [Authorize(Policy = Constants.Authorization.Policy.ClientAdmin)]
    [Authorize(Policy = Constants.Authorization.Policy.Operator)]
    [HttpPost]
    public async Task<IActionResult> AddCustomer([FromBody] CreateCustomerDTO createCustomerDto)
    {
        var result = await sender.Send(new CreateCustomerRequest(createCustomerDto));

        return FromResult(result);
    }

    [Authorize(Policy = Constants.Authorization.Policy.Owner)]
    [Authorize(Policy = Constants.Authorization.Policy.ClientAdmin)]
    [Authorize(Policy = Constants.Authorization.Policy.Operator)]
    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        var result = await sender.Send(new GetCustomersRequest());
        return FromResult(result);
    }

    [Authorize(Policy = Constants.Authorization.Policy.Owner)]
    [Authorize(Policy = Constants.Authorization.Policy.ClientAdmin)]
    [Authorize(Policy = Constants.Authorization.Policy.Operator)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCustomerById([FromRoute] Guid id)
    {
        var result = await sender.Send(new GetCustomerByIdRequest(id));
        return FromResult(result);
    }

    [Authorize(Policy = Constants.Authorization.Policy.Owner)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCustomer([FromRoute] Guid id)
    {
        var result = await sender.Send(new DeleteCustomerRequest(id));
        return FromResult(result);
    }

    [Authorize(Policy = Constants.Authorization.Policy.Owner)]
    [Authorize(Policy = Constants.Authorization.Policy.ClientAdmin)]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCustomer([FromRoute] Guid id, [FromBody] UpdateCustomerDTO updateCustomerDTO)
    {
        var result = await sender.Send(new UpdateCustomerRequest(id, updateCustomerDTO));
        return FromResult(result);
    }
}
