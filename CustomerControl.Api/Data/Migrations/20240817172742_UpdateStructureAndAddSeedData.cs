using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CustomerControl.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStructureAndAddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password" },
                values: new object[,]
                {
                    { 1, "alice@example.com", "Alice", "+TDCQANLq7qeMUjxCg0QvJrX0qnY0WbD2Lbv7EJF3bCYMg1sQbN3RHZr7RRhkipN" },
                    { 2, "bob@example.com", "Bob", "dImTgkU5lr/RFZRW9N5ysfu0k6wLjx+ib7epmwwNUX3FHi9IjdyUvzFqEx4d0nHb" }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "Document", "Name", "Phone", "UserId" },
                values: new object[,]
                {
                    { 1, "Rua Nova", "12345678901", "Customer 1", "5551234567", 1 },
                    { 2, "Rua Alta", "98765432100", "Customer 2", "5559876543", 2 }
                });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "Id", "Amount", "CustomerId", "Description", "DueDate", "Paid" },
                values: new object[,]
                {
                    { 1, 100.00m, 1, "Invoice 1", new DateTime(2024, 9, 16, 17, 27, 41, 917, DateTimeKind.Utc).AddTicks(9158), false },
                    { 2, 200.00m, 2, "Invoice 2", new DateTime(2024, 9, 16, 17, 27, 41, 917, DateTimeKind.Utc).AddTicks(9264), true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
