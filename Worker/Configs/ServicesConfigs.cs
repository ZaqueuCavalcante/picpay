using PicPay.Api.Configs;
using PicPay.Api.Settings;
using PicPay.Worker.Events;

namespace PicPay.Worker.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this IServiceCollection services)
    {
        services.AddEfCoreConfigs();
        services.AddSingleton<DatabaseSettings>();

        services.AddTransient<DomainEventsProcessor>();
    }
}
