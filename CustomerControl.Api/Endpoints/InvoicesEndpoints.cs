using CustomerControl.Api.Data;
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

        return group;
    }
}
