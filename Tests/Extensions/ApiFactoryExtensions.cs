using PicPay.Api.Database;
using Microsoft.Extensions.DependencyInjection;

namespace PicPay.Tests.Extensions;

public static class ApiFactoryExtensions
{
    public static HttpClient GetClient(this ApiFactory factory)
    {
        return factory.CreateClient();
    }

    public static PicPayDbContext GetDbContext(this ApiFactory factory)
    {
        var scope = factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<PicPayDbContext>();
    }
}
