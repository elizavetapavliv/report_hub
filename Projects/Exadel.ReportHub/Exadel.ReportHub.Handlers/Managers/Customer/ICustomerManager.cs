using Exadel.ReportHub.SDK.DTOs.Customer;

namespace Exadel.ReportHub.Handlers.Managers.Customer;

public interface ICustomerManager
{
    Task<CustomerDTO> GenerateCustomerAsync(CreateCustomerDTO createCustomerDto, CancellationToken cancellationToken);

    Task<IList<CustomerDTO>> GenerateCustomersAsync(IEnumerable<CreateCustomerDTO> createCustomerDtos, CancellationToken cancellationToken);
}
