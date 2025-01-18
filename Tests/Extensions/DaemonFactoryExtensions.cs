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

    public static async Task AwaitEventsProcessing(this WorkerFactory factory)
    {
        await using var ctx = factory.GetDbContext();
        var count = 0;
        while (true)
        {
            if (count == 2) break;

            var events = await ctx.Events.CountAsync(x => x.ProcessedAt == null);
            if (events == 0) break;
            await Task.Delay(500);
            count ++;
        }
    }

    public static async Task AwaitTasksProcessing(this WorkerFactory factory)
    {
        await using var ctx = factory.GetDbContext();
        var count = 0;
        while (true)
        {
            if (count == 2) break;

            var tasks = await ctx.Tasks.CountAsync(x => x.ProcessedAt == null);
            if (tasks == 0) break;
            await Task.Delay(500);
            count ++;
        }
    }

    public static T GetService<T>(this WorkerFactory factory) where T : notnull
    {
        var scope = factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>();
    }
}
