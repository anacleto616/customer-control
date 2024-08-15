using CustomerControl.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomerControl.Api.Data;

public class CustomerControlContext(DbContextOptions<CustomerControlContext> options)
    : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Invoice> Invoices => Set<Invoice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<User>()
            .HasMany(u => u.Customers)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId);

        modelBuilder
            .Entity<Customer>()
            .HasMany(c => c.Invoices)
            .WithOne(i => i.Customer)
            .HasForeignKey(i => i.CustomerId);

        base.OnModelCreating(modelBuilder);
    }
}
