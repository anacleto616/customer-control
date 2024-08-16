using CustomerControl.Api.Data;
using CustomerControl.Api.Dtos;
using CustomerControl.Api.Entities;
using CustomerControl.Api.Mapping;
using CustomerControl.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace CustomerControl.Api.Endpoints;

public static class UsersEndpoints
{
    public static RouteGroupBuilder MapUsersEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("users").WithParameterValidation();

        // POST /users/register
        group.MapPost(
            "/register",
            async (
                UserRegisterDto newUser,
                Argon2PasswordHasher hasher,
                CustomerControlContext dbContext
            ) =>
            {
                var existingUser = await dbContext.Users.AnyAsync(u => u.Email == newUser.Email);

                if (existingUser)
                {
                    return Results.BadRequest(
                        "Este e-mail já está sendo utilizado por outro usuário."
                    );
                }

                User user = newUser.ToEntity();

                user.Password = hasher.HashPassword(user.Password);

                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();

                return Results.Ok("Usuário criado com sucesso.");
            }
        );

        return group;
    }
}
