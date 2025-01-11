namespace PicPay.Api.Configs;

public static class HttpConfigs
{
    public static void AddHttpConfigs(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddRouting(options => options.LowercaseUrls = true);
    }
}
