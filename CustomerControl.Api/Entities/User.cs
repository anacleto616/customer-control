namespace CustomerControl.Api.Entities;

public class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }

    public IEnumerable<Customer>? Customers { get; set; }
}
