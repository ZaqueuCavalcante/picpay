namespace PicPay.Api.Configs;

public static class AuthorizationConfigs
{
    public static void AddAuthorizationConfigs(this IServiceCollection services)
    {
        services.AddAuthorization();
    }
}
