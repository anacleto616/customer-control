using CustomerControl.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomerControl.Api.Endpoints
{
    public static class CustomersEndpoints
    {
        const string GetCustomerEndpointName = "GetCustomer";

        public static RouteGroupBuilder MapCustomersEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("customers").WithParameterValidation();

            // GET /customers
            group.MapGet(
                "/",
                async (CustomerControlContext dbContext) =>
                    await dbContext
                        .Customers.Select(customer => customer)
                        .AsNoTracking()
                        .ToListAsync()
            );

            // DELETE /customers/1
            group.MapDelete(
                "/{id}",
                async (int id, CustomerControlContext dbContext) =>
                {
                    await dbContext
                        .Customers.Where(customer => customer.Id == id)
                        .ExecuteDeleteAsync();

                    return Results.NoContent();
                }
            );

            return group;
        }
    }
}
