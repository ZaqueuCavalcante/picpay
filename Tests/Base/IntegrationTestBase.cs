using PicPay.Api.Database;
using Microsoft.Extensions.DependencyInjection;

namespace PicPay.Tests.Base;

public class IntegrationTestBase
{
    protected ApiFactory _api = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _api = new ApiFactory();
        using var scope = _api.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<PicPayDbContext>();

        await ctx.ResetDbAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _api.DisposeAsync();
    }
}
