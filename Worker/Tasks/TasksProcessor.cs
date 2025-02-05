using Newtonsoft.Json;
using PicPay.Api.Tasks;
using System.Diagnostics;
using PicPay.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace PicPay.Worker.Tasks;

public class TasksProcessor(IServiceScopeFactory serviceScopeFactory)
{
    public async Task Run()
    {
        using var scope = serviceScopeFactory.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<PicPayDbContext>();

        await Process(scope, ctx, Guid.NewGuid());
    }

    private static async Task Process(IServiceScope scope, PicPayDbContext ctx, Guid processorId)
    {   
        var tasks = await ctx.Tasks.FromSqlRaw(Sql, processorId).ToListAsync();
        if (tasks.Count == 0) return;

        var sw = Stopwatch.StartNew();

        foreach (var task in tasks)
        {
            sw.Restart();

            ctx.Attach(task);
            await ctx.Database.BeginTransactionAsync();

            try
            {
                dynamic data = GetData(task);
                dynamic handler = GetHandler(scope, task);
                await handler.Handle(data);
            }
            catch (Exception ex)
            {
                ctx.ChangeTracker.Clear();
                ctx.Attach(task);
                task.Error = ex.Message + ex.InnerException?.Message;
            }

            sw.Stop();

            task.Processed(sw.Elapsed.TotalMilliseconds);

            await ctx.SaveChangesAsync();
            ctx.ChangeTracker.Clear();
            await ctx.Database.CommitTransactionAsync();
        }

        await Process(scope, ctx, processorId);
    }

    private static dynamic GetData(PicPayTask task)
    {
        var type = typeof(PicPayTask).Assembly.GetType(task.Type)!;
        dynamic data = JsonConvert.DeserializeObject(task.Data, type)!;
        return data;
    }

    private static dynamic GetHandler(IServiceScope scope, PicPayTask task)
    {
        var handlerType = typeof(IPicPayTask).Assembly.GetType($"{task.Type}Handler")!;
        dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);
        return handler;
    }

    private static readonly string Sql = @"
        UPDATE picpay.tasks
        SET processor_id = {0}, status = 'Processing'
        WHERE id IN (
            SELECT id
            FROM picpay.tasks
            WHERE processor_id IS NULL
            ORDER BY created_at
            LIMIT 100
            FOR UPDATE SKIP LOCKED
        );

        SELECT *
        FROM picpay.tasks
        WHERE processor_id = {0} AND processed_at IS NULL
        ORDER BY created_at;
    ";
}
