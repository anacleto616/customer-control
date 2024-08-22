using CustomerControl.Api.Data;
using CustomerControl.Api.Dtos;
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
            global::System.Object value = group.MapGet(
                "/all/{userId}",
                async (int userId, CustomerControlContext dbContext) =>
                    await dbContext
                        .Customers.Where(customer => customer.UserId == userId)
                        .Include(customer => customer.Invoices)
                        .Select(customer => customer.ToCustomerDetailsDto())
                        .AsNoTracking()
                        .ToListAsync()
            ).RequireAuthorization();

            // GET /customers/1
            group
                .MapGet(
                    "/{id}",
                    async (int id, CustomerControlContext dbContext) =>
                    {
                        Customer? customer = await dbContext.Customers.FindAsync(id);

                        return customer is null
                            ? Results.NotFound()
                            : Results.Ok(customer.ToCustomerSummaryDto());
                    }
                )
                .WithName(GetCustomerEndpointName)
                .RequireAuthorization();

            // POST /customers
            group.MapPost(
                "/",
                async (CreateCustomerDto newCustomer, CustomerControlContext dbContext) =>
                {
                    Customer customer = newCustomer.ToEntity();

                    dbContext.Customers.Add(customer);
                    await dbContext.SaveChangesAsync();

                    return Results.CreatedAtRoute(
                        GetCustomerEndpointName,
                        new { id = customer.Id },
                        customer.ToCustomerSummaryDto()
                    );
                }
            ).RequireAuthorization();

            // PUT /customers/1
            group.MapPut(
                "/{id}",
                async (
                    int id,
                    UpdateCustomerDto updatedCustomer,
                    CustomerControlContext dbContext
                ) =>
                {
                    var existingCustomer = await dbContext.Customers.FindAsync(id);

                    if (existingCustomer is null)
                    {
                        return Results.NotFound();
                    }

                    dbContext
                        .Entry(existingCustomer)
                        .CurrentValues.SetValues(updatedCustomer.ToEntity(id));

                    await dbContext.SaveChangesAsync();

                    return Results.NoContent();
                }
            ).RequireAuthorization();

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
            ).RequireAuthorization();z;

            return group;
        }
    }
}
