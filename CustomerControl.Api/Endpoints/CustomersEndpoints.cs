using CustomerControl.Api.Data;
using CustomerControl.Api.Entities;
using CustomerControl.Api.Mapping;
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
                        .Customers.Select(customer => customer.ToCustomerDetailsDto())
                        .AsNoTracking()
                        .ToListAsync()
            );

            // GET /customers/1
            group
                .MapGet(
                    "/{id}",
                    async (int id, CustomerControlContext dbContext) =>
                    {
                        Customer? customer = await dbContext.Customers.FindAsync(id);

                        return customer is null
                            ? Results.NotFound()
                            : Results.Ok(customer.ToCustomerSummarysDto());
                    }
                )
                .WithName(GetCustomerEndpointName);

            // POST /customers
            group.MapPost(
                "/",
                async (Customer newCustomer, CustomerControlContext dbContext) =>
                {
                    dbContext.Customers.Add(newCustomer);
                    await dbContext.SaveChangesAsync();

                    return Results.CreatedAtRoute(
                        GetCustomerEndpointName,
                        new { id = newCustomer.Id },
                        newCustomer
                    );
                }
            );

            // PUT /customers/1
            group.MapPut(
                "/{id}",
                async (int id, Customer updatedCustomer, CustomerControlContext dbContext) =>
                {
                    var existingCustomer = await dbContext.Customers.FindAsync(id);

                    if (existingCustomer is null)
                    {
                        return Results.NotFound();
                    }

                    dbContext.Entry(existingCustomer).CurrentValues.SetValues(updatedCustomer);
                    await dbContext.SaveChangesAsync();

                    return Results.NoContent();
                }
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
