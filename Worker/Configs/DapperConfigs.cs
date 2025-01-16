using Dapper;

namespace PicPay.Worker.Configs;

public static class DapperConfigs
{
    public static void AddDapperConfigs(this IServiceCollection _)
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }
}
