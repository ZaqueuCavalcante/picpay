namespace PicPay.Api.Configs;

public static class SettingsConfigs
{
    public static void AddSettingsConfigs(this IServiceCollection services)
    {
        services.AddSingleton<AuthSettings>();
        services.AddSingleton<NotifySettings>();
        services.AddSingleton<DatabaseSettings>();
        services.AddSingleton<AuthorizeSettings>();
        services.AddSingleton<RateLimiterSettings>();
    }
}
