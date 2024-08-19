using System.Text.Json.Serialization;
using CustomerControl.Api.Data;
using CustomerControl.Api.Endpoints;
using CustomerControl.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load("../.env.development");

var connectionString =
    $"Host={Environment.GetEnvironmentVariable("POSTGRES_HOST")};"
    + $"Port={Environment.GetEnvironmentVariable("POSTGRES_PORT")};"
    + $"Database={Environment.GetEnvironmentVariable("POSTGRES_DB")};"
    + $"Username={Environment.GetEnvironmentVariable("POSTGRES_USER")};"
    + $"Password={Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")}";

builder.Services.AddDbContext<CustomerControlContext>(options =>
    options.UseNpgsql(connectionString)
);

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

builder.Services.AddSingleton<Argon2PasswordHasher>();

builder.Services.AddCors(options =>
    options.AddPolicy(
        "AllowLocalhost4200",
        builder => builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod()
    )
);

var app = builder.Build();

app.UseCors("AllowLocalhost4200");

app.MapUsersEndpoints();
app.MapCustomersEndpoints();
app.MapInvoicesEndpoints();

await app.MigrateDbAsync();

app.Run();
