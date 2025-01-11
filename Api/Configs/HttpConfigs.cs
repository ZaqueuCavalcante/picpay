using System.Text.Json.Serialization;

namespace PicPay.Api.Configs;

public static class HttpConfigs
{
    public static void AddHttpConfigs(this IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        services.AddRouting(options => options.LowercaseUrls = true);
    }
}
