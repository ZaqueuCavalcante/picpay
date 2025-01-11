using Serilog;

namespace PicPay.Api.Configs;

public static class LoggingConfigs
{
    public static void AddLoggingConfigs(this ConfigureHostBuilder builder)
    {
        builder.UseSerilog((context, config) => 
            config.ReadFrom.Configuration(context.Configuration)
        );
    }
}
