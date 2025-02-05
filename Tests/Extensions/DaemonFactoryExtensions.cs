using PicPay.Api.Database;
using Microsoft.Extensions.DependencyInjection;

namespace PicPay.Tests.Extensions;

public static class WorkerFactoryExtensions
{
    public static PicPayDbContext GetDbContext(this WorkerFactory factory)
    {
        var scope = factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<PicPayDbContext>();
    }

    public static async Task ProcessAll(this WorkerFactory factory)
    {
        await Task.Delay(1);
        // await factory.AwaitEventsProcessing();
        // await factory.AwaitTasksProcessing();
    }

    public static async Task AwaitEventsProcessing(this WorkerFactory factory)
    {
        await using var ctx = factory.GetDbContext();
        while (true)
        {
            var events = await ctx.Events.CountAsync(x => x.ProcessedAt == null);
            if (events == 0) break;
            await Task.Delay(500);
        }
    }

    public static async Task AwaitTasksProcessing(this WorkerFactory factory)
    {
        await using var ctx = factory.GetDbContext();
        while (true)
        {
            var tasks = await ctx.Tasks.CountAsync(x => x.ProcessedAt == null);
            if (tasks == 0) break;
            await Task.Delay(500);
        }
    }

    public static T GetService<T>(this WorkerFactory factory) where T : notnull
    {
        var scope = factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>();
    }
}
