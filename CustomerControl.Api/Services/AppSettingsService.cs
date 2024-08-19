using CustomerControl.Api.Entities;

namespace CustomerControl.Api.Services;

public static class AppSettingsService
{
    public static JwtSettings JwtSettings { get; }

    static AppSettingsService()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        JwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>() ?? new JwtSettings();
    }
}
