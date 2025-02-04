using PicPay.Api.Database;
using PicPay.Api.Extensions;
using PicPay.Tests.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace PicPay.Tests.Base;

public class IntegrationTestBase
{
    protected ApiFactory _api = null!;
    protected WorkerFactory _worker = null!;
    protected VendorsFactory _vendors = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        Env.SetAsTesting();

        _api = new ApiFactory();
        using var scope = _api.Services.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<PicPayDbContext>();

        await ctx.ResetDbAsync();
        await _api.RegisterAdm();

        _worker = new WorkerFactory();
        using var _ = _worker.Services.CreateScope();

        _vendors = new VendorsFactory();
        using var __ = _vendors.Services.CreateScope();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await using var ctx = _api.GetDbContext();

        var sum = await ctx.Wallets.SumAsync(w => w.Balance);
        sum.Should().Be(0);

        await _api.DisposeAsync();
        await _worker.DisposeAsync();
        await _vendors.DisposeAsync();
    }
}
