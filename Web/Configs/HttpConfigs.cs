namespace PicPay.Web.Configs;

public static class HttpConfigs
{
    public static void AddHttpConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<PicPayDelegatingHandler>();

        var apiUrl = builder.Configuration.GetSection("ApiUrl").Value!;

        builder.Services
            .AddHttpClient("HttpClient", x => x.BaseAddress = new Uri(apiUrl))
            .AddHttpMessageHandler<PicPayDelegatingHandler>();

        builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>()
            .CreateClient("HttpClient"));
    }
}
