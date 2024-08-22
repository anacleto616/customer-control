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
            "/all/{customerId}",
            async (int customerId, CustomerControlContext dbContext) =>
                await dbContext
                    .Invoices.Where(invoice => invoice.Id == customerId)
                    .Select(invoice => invoice.ToInvoiceDetailsDto())
                    .AsNoTracking()
                    .ToListAsync()
        ).RequireAuthorization();

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
            ).RequireAuthorization();
            .WithName(GetInvoiceEndpointName);

        // POST /invoices
        group.MapPost(
            "/",
            async (CreateInvoiceDto newInvoice, CustomerControlContext dbContext) =>
            {
                Invoice invoice = newInvoice.ToEntity();

                dbContext.Invoices.Add(invoice);
                await dbContext.SaveChangesAsync();

                return Results.CreatedAtRoute(
                    GetInvoiceEndpointName,
                    new { id = invoice.Id },
                    invoice.ToInvoiceSummaryDto()
                );
            }
        ).RequireAuthorization();

        // PUT /invoices/1
        group.MapPut(
            "/{id}",
            async (int id, UpdateInvoiceDto updatedInvoice, CustomerControlContext dbContext) =>
            {
                var existingInvoice = await dbContext.Invoices.FindAsync(id);

                if (existingInvoice is null)
                {
                    return Results.NotFound();
                }

                dbContext
                    .Entry(existingInvoice)
                    .CurrentValues.SetValues(updatedInvoice.ToEntity(id));
                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            }
        ).RequireAuthorization();

        // DELETE /invoice/1
        group.MapDelete(
            "/{id}",
            async (int id, CustomerControlContext dbContext) =>
            {
                await dbContext.Invoices.Where(invoice => invoice.Id == id).ExecuteDeleteAsync();

                return Results.NoContent();
            }
        ).RequireAuthorization();

        return group;
    }
}
