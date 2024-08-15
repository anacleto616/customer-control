namespace CustomerControl.Api.Entities;

public class Invoice
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public bool Paid { get; set; }

    public int CustomerId { get; set; }

    public required Customer Customer { get; set; }
}
