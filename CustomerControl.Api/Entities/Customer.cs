namespace CustomerControl.Api.Entities;

public class Customer
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Document { get; set; }
    public required string Phone { get; set; }
    public required string Address { get; set; }

    public int UserId { get; set; }

    public User? User { get; set; }
    public IEnumerable<Invoice>? Invoices { get; set; }
}
