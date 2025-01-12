using Microsoft.AspNetCore.Identity;

namespace PicPay.Api.Configs;

public static class IdentityConfigs
{
    public static void AddIdentityConfigs(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher<IdentityUser>, PasswordHasher<IdentityUser>>();
    }
}
