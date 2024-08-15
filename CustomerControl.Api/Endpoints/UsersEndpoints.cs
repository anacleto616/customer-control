using CustomerControl.Api.Data;
using CustomerControl.Api.Entities;

namespace CustomerControl.Api.Endpoints;

public static class UsersEndpoints
{
    public static RouteGroupBuilder MapUsersEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("users").WithParameterValidation();

        // POST /users/register
        group.MapPost(
            "/register",
            async (User user, CustomerControlContext dbContext) =>
            {
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();

                return Results.Ok("Usuário criado com sucesso.");
            }
        );

        return group;
    }
}
