using PicPay.Api.Settings;
using PicPay.Worker.Settings;

namespace PicPay.Worker.Extensions;

public static class SettingsExtensions
{
    public static DatabaseSettings Database(this IConfiguration configuration) => new(configuration);
    public static HangfireSettings Hangfire(this IConfiguration configuration) => new(configuration);
}
