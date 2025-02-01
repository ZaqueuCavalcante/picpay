using Dapper;

namespace PicPay.Api.Configs;

public static class EfCoreConfigs
{
    public static void AddEfCoreConfigs(this IServiceCollection services)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddDbContext<PicPayDbContext>();
    }
}
