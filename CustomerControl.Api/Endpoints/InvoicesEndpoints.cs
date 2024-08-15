using CustomerControl.Api.Data;
using CustomerControl.Api.Entities;
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
                await dbContext.Invoices.Select(invoices => invoices).AsNoTracking().ToListAsync()
        );

        // GET /invoices/1
        group
            .MapGet(
                "/{id}",
                async (int id, CustomerControlContext dbContext) =>
                {
                    Invoice? invoice = await dbContext.Invoices.FindAsync(id);

                    return invoice is null ? Results.NotFound() : Results.Ok(invoice);
                }
            )
            .WithName(GetInvoiceEndpointName);

        return group;
    }
}
