using System.Diagnostics.CodeAnalysis;
using Exadel.ReportHub.Handlers.Customer.Create;
using Exadel.ReportHub.Handlers.Customer.Delete;
using Exadel.ReportHub.Handlers.Customer.Get;
using Exadel.ReportHub.Handlers.Customer.GetById;
using Exadel.ReportHub.Handlers.Customer.Update;
using Exadel.ReportHub.Host.Infrastructure.Models;
using Exadel.ReportHub.Host.Services.Abstract;
using Exadel.ReportHub.SDK.DTOs.Customer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exadel.ReportHub.Host.Services;

[ExcludeFromCodeCoverage]
[Route("api/customers")]
public class CustomersService(ISender sender) : BaseService
{
    /// <summary>
    /// Creates a new customer.
    /// </summary>
    /// <param name="createCustomerDto"></param>
    /// <returns>The newly created customer details.</returns>
    /// <response code="200">Customer created successfully.</response>
    /// <response code="400">Invalid input provided.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. Users who do not have permission to perform this action.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Create)]
    [HttpPost]
    [ProducesResponseType(typeof(CustomerDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<CustomerDTO>> AddCustomer([FromBody] CreateCustomerDTO createCustomerDto)
    {
        var result = await sender.Send(new CreateCustomerRequest(createCustomerDto));

        return FromResult(result);
    }

    /// <summary>
    /// Retrieves the list of all customers.
    /// </summary>
    /// <returns>A list of customers.</returns>
    /// <response code="200">List of Customers returned successfully.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. Users who not have permission to perform this action.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Read)]
    [HttpGet]
    [ProducesResponseType(typeof(IList<CustomerDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IList<CustomerDTO>>> GetCustomers()
    {
        var result = await sender.Send(new GetCustomersRequest());
        return FromResult(result);
    }

    /// <summary>
    /// Retrieves a customer by their unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>The customer details if found.</returns>
    /// <response code="200">Customer found and returned successfully.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. Users who do not have permission to perform this action.</response>
    /// <response code="404">Customer with the given ID was not found.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Read)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CustomerDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDTO>> GetCustomerById([FromRoute] Guid id)
    {
        var result = await sender.Send(new GetCustomerByIdRequest(id));
        return FromResult(result);
    }

    /// <summary>
    /// Updates the details of an existing customer.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updateCustomerDTO"></param>
    /// <returns>No content</returns>
    /// <response code="204">Customer name updated successfully.</response>
    /// <response code="400">Invalid name provided.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. Users who do not have permission to perform this action.</response>
    /// <response code="404">Customer with the given ID was not found.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Update)]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateCustomer([FromRoute] Guid id, [FromBody] UpdateCustomerDTO updateCustomerDTO)
    {
        var result = await sender.Send(new UpdateCustomerRequest(id, updateCustomerDTO));
        return FromResult(result);
    }

    /// <summary>
    /// Deletes an existing customer by their unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>No content</returns>
    /// <response code="204">Customer deleted successfully.</response>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Forbidden. Users who do not have permission to perform this action.</response>
    /// <response code="404">Customer with the given ID was not found.</response>
    [Authorize(Policy = Constants.Authorization.Policy.Delete)]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteCustomer([FromRoute] Guid id)
    {
        var result = await sender.Send(new DeleteCustomerRequest(id));
        return FromResult(result);
    }
}
