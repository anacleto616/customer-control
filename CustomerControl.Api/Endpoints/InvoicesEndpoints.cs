using CustomerControl.Api.Data;
using CustomerControl.Api.Dtos;
using CustomerControl.Api.Entities;
using CustomerControl.Api.Mapping;
using Microsoft.EntityFrameworkCore;

namespace CustomerControl.Api.Endpoints;

public static class InvoicesEndpoints
{
    const string GetInvoiceEndpointName = "GetInvoice";

    public static RouteGroupBuilder MapInvoicesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("invoices").WithParameterValidation();

        // GET /invoices
        group.MapGet(
            "/",
            async (CustomerControlContext dbContext) =>
                await dbContext
                    .Invoices.Select(invoice => invoice.ToInvoiceDetailsDto())
                    .AsNoTracking()
                    .ToListAsync()
        );

        // GET /invoices/1
        group
            .MapGet(
                "/{id}",
                async (int id, CustomerControlContext dbContext) =>
                {
                    Invoice? invoice = await dbContext.Invoices.FindAsync(id);

                    return invoice is null
                        ? Results.NotFound()
                        : Results.Ok(invoice.ToInvoiceSummaryDto());
                }
            )
            .WithName(GetInvoiceEndpointName);

        // POST /invoices
        group.MapPost(
            "/",
            async (CreateInvoiceDto newInvoice, int customerId, CustomerControlContext dbContext) =>
            {
                var customer =
                    await dbContext.Customers.FindAsync(customerId)
                    ?? throw new ArgumentException("Customer not found.");

                Invoice invoice = newInvoice.ToEntity(customer);

                dbContext.Invoices.Add(invoice);
                await dbContext.SaveChangesAsync();

                return Results.CreatedAtRoute(
                    GetInvoiceEndpointName,
                    new { id = invoice.Id },
                    invoice.ToInvoiceSummaryDto()
                );
            }
        );

        // PUT /invoices/1
        group.MapPut(
            "/{id}",
            async (
                int id,
                int customerId,
                UpdateInvoiceDto updatedInvoice,
                CustomerControlContext dbContext
            ) =>
            {
                var existingInvoice = await dbContext.Invoices.FindAsync(id);

                if (existingInvoice is null)
                {
                    return Results.NotFound();
                }

                var customer =
                    await dbContext.Customers.FindAsync(customerId)
                    ?? throw new ArgumentException("Customer not found.");

                dbContext
                    .Entry(existingInvoice)
                    .CurrentValues.SetValues(updatedInvoice.ToEntity(id, customer));
                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            }
        );

        // DELETE /invoice/1
        group.MapDelete(
            "/{id}",
            async (int id, CustomerControlContext dbContext) =>
            {
                await dbContext.Invoices.Where(invoice => invoice.Id == id).ExecuteDeleteAsync();

                return Results.NoContent();
            }
        );

        return group;
    }
}
