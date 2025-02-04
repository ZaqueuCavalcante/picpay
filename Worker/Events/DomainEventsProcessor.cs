using Newtonsoft.Json;
using PicPay.Api.Events;
using System.Diagnostics;
using PicPay.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace PicPay.Worker.Events;

public class DomainEventsProcessor(IServiceScopeFactory serviceScopeFactory)
{
    public async Task Run()
    {
        using var scope = serviceScopeFactory.CreateScope();
        var ctx = scope.ServiceProvider.GetRequiredService<PicPayDbContext>();

        await Process(scope, ctx, Guid.NewGuid());
    }

    private static async Task Process(IServiceScope scope, PicPayDbContext ctx, Guid processorId)
    {
        var events = await ctx.Events.FromSqlRaw(Sql, processorId).ToListAsync();
        if (events.Count == 0) return;

        await ctx.Database.BeginTransactionAsync();
        var sw = Stopwatch.StartNew();

        foreach (var evt in events)
        {
            sw.Restart();

            try
            {
                dynamic data = GetData(evt);
                dynamic handler = GetHandler(scope, evt);
                await handler.Handle(evt.Id, data);
            }
            catch (Exception ex)
            {
                evt.Error = ex.Message + ex.InnerException?.Message;
            }

            sw.Stop();

            evt.Processed(sw.Elapsed.TotalMilliseconds);
        }

        await ctx.SaveChangesAsync();
        await ctx.Database.CommitTransactionAsync();

        await Process(scope, ctx, processorId);
    }

    private static dynamic GetData(DomainEvent evt)
    {
        var type = typeof(DomainEvent).Assembly.GetType(evt.Type)!;
        dynamic data = JsonConvert.DeserializeObject(evt.Data, type)!;
        return data;
    }

    private static dynamic GetHandler(IServiceScope scope, DomainEvent evt)
    {
        var handlerType = typeof(IDomainEvent).Assembly.GetType($"{evt.Type}Handler")!;
        dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);
        return handler;
    }

    private static readonly string Sql = @"
        UPDATE picpay.domain_events
        SET processor_id = {0}, status = 'Processing'
        WHERE id IN (
            SELECT id
            FROM picpay.domain_events
            WHERE processor_id IS NULL
            ORDER BY occurred_at
            LIMIT 1000
            FOR UPDATE SKIP LOCKED
        );

        SELECT *
        FROM picpay.domain_events
        WHERE processor_id = {0} AND processed_at IS NULL
        ORDER BY occurred_at;
    ";
}
