using System.Text;
using System.Text.Json.Serialization;
using CustomerControl.Api.Data;
using CustomerControl.Api.Endpoints;
using CustomerControl.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(AppSettingsService.JwtSettings.Key)
            ),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

builder.Services.AddCors(options =>
    options.AddPolicy(
        "AllowLocalhost4200",
        builder => builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod()
    )
);

var app = builder.Build();

app.UseCors("AllowLocalhost4200");

app.UseAuthentication();
app.UseAuthorization();

app.MapUsersEndpoints();
app.MapCustomersEndpoints();
app.MapInvoicesEndpoints();

await app.MigrateDbAsync();

app.Run();
