using CustomerControl.Api.Entities;
using CustomerControl.Api.Services;
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
        var hasher = new Argon2PasswordHasher();

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

        modelBuilder
            .Entity<User>()
            .HasData(
                new
                {
                    Id = 1,
                    Name = "Alice",
                    Email = "alice@example.com",
                    Password = hasher.HashPassword("Alice@2024")
                },
                new
                {
                    Id = 2,
                    Name = "Bob",
                    Email = "bob@example.com",
                    Password = hasher.HashPassword("Bob@2024")
                }
            );

        modelBuilder
            .Entity<Customer>()
            .HasData(
                new Customer
                {
                    Id = 1,
                    Name = "Customer 1",
                    Document = "12345678901",
                    Phone = "5551234567",
                    Address = "Rua Nova",
                    UserId = 1
                },
                new Customer
                {
                    Id = 2,
                    Name = "Customer 2",
                    Document = "98765432100",
                    Phone = "5559876543",
                    Address = "Rua Alta",
                    UserId = 2
                }
            );

        modelBuilder
            .Entity<Invoice>()
            .HasData(
                new Invoice
                {
                    Id = 1,
                    Description = "Invoice 1",
                    Amount = 100.00m,
                    DueDate = DateTime.Now.AddDays(30).ToUniversalTime(),
                    Paid = false,
                    CustomerId = 1
                },
                new Invoice
                {
                    Id = 2,
                    Description = "Invoice 2",
                    Amount = 200.00m,
                    DueDate = DateTime.Now.AddDays(45).ToUniversalTime(),
                    Paid = true,
                    CustomerId = 2
                }
            );

        base.OnModelCreating(modelBuilder);
    }
}
