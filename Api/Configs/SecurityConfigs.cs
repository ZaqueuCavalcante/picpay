using PicPay.Api.Security;

namespace PicPay.Api.Configs;

public static class SecurityConfigs
{
    public static void AddSecurityConfigs(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
    }
}
