namespace PicPay.Api;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSettingsConfigs();
        services.AddServicesConfigs();

        services.AddIdentityConfigs();
        services.AddSecurityConfigs();

        services.AddAuthenticationConfigs();
        services.AddAuthorizationConfigs();

        services.AddHttpConfigs();
        services.AddEfCoreConfigs();

        services.AddOpenApi();
        services.AddDocsConfigs();
        services.AddCorsConfigs();

        services.AddRateLimiterConfigs();
    }

    public static void Configure(IApplicationBuilder app)
    {
        app.UseCors();
        app.UseRouting();
        app.UseRateLimiter();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseStaticFiles();
        app.UseControllers();

        app.UseLogging();
        app.UseSwagger();
    }
}
