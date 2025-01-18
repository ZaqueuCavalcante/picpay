namespace PicPay.Web.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddServiceConfigs(typeof(IPicPayClient));
    }

    private static void AddServiceConfigs(this IServiceCollection services, Type? marker)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName.StartsWith("Web"))
            .SelectMany(s => s.GetTypes())
            .Where(p => marker.IsAssignableFrom(p) && !p.IsInterface)
            .ToList();

        foreach (var type in types)
        {
            services.AddScoped(type);
        }
    }
}
