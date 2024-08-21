using CustomerControl.Api.Data;
using CustomerControl.Api.Dtos;
using CustomerControl.Api.Entities;
using CustomerControl.Api.Mapping;
using CustomerControl.Api.Services;
using Microsoft.AspNetCore.Authorization;
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
            [AllowAnonymous]
            async (UserRegisterDto newUser, CustomerControlContext dbContext) =>
            {
                var existingUser = await dbContext.Users.AnyAsync(user =>
                    user.Email == newUser.Email
                );

                if (existingUser)
                {
                    return Results.BadRequest(
                        "Este e-mail já está sendo utilizado por outro usuário."
                    );
                }

                User user = newUser.ToEntity();

                user.Password = BCryptService.HashPassword(user.Password);

                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();

                return Results.Ok("Usuário criado com sucesso.");
            }
        );

        // POST /users/login
        group.MapPost(
            "/login",
            [AllowAnonymous]
            async (UserLoginDto userLogin, CustomerControlContext dbContext) =>
            {
                var user = await dbContext
                    .Users.Where(user => user.Email == userLogin.Email)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return Results.Unauthorized();
                }

                var passwordMatches = BCryptService.VerifyPassword(
                    userLogin.Password,
                    user.Password
                );

                var token = JwtBearerService.GenerateToken(user);

                var response = new LoginResponseDto(token, user.Id);

                return !passwordMatches ? Results.Unauthorized() : Results.Ok(response);
            }
        );

        return group;
    }
}
