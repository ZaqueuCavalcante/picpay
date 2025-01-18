using PicPay.Api.Configs;
using PicPay.Api.Settings;
using PicPay.Worker.Events;
using PicPay.Api.Features.Cross.Notify;

namespace PicPay.Worker.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this IServiceCollection services)
    {
        services.AddEfCoreConfigs();
        services.AddSingleton<NotifySettings>();
        services.AddSingleton<DatabaseSettings>();

        services.AddTransient<NotifyService>();

        services.AddTransient<DomainEventsProcessor>();

        services.AddHttpClient();
    }
}
