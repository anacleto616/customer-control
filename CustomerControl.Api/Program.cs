using CustomerControl.Api.Data;
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

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

await app.MigrateDbAsync();

app.Run();
