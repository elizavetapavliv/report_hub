namespace Exadel.ReportHub.Data.Models;

public class Client : IDocument
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public IList<Customer> Customers { get; set; }
}
