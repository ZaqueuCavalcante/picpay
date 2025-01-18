using Microsoft.AspNetCore.Components.Authorization;

namespace PicPay.Web.Configs;

public static class AuthConfigs
{
    public static void AddAuthConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddAuthorizationCore();

        builder.Services.AddScoped<PicPayAuthStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<PicPayAuthStateProvider>());
    }
}
