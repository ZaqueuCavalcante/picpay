using PicPay.Api.Database;
using PicPay.Tests.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace PicPay.Tests.Base;

public class IntegrationTestBase
{
    protected ApiFactory _api = null!;
    protected AuthFactory _auth = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _api = new ApiFactory();
        using var scope = _api.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<PicPayDbContext>();

        _auth = new AuthFactory();
        using var _ = _auth.Services.CreateScope();

        await ctx.ResetDbAsync();
        await _api.RegisterAdm();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await using var ctx = _api.GetDbContext();

        var sum = await ctx.Wallets.SumAsync(w => w.Balance);
        sum.Should().Be(0);

        await _api.DisposeAsync();
        await _auth.DisposeAsync();
    }
}
