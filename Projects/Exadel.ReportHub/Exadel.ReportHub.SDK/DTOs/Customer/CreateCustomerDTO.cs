namespace Exadel.ReportHub.SDK.DTOs.Customer;

public class CreateCustomerDTO
{
    public string Email { get; set; }

    public string Name { get; set; }

    public Guid CountryId { get; set; }
}
