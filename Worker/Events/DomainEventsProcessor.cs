using Dapper;
using Npgsql;
using Hangfire;
using PicPay.Shared;
using Newtonsoft.Json;
using PicPay.Api.Events;
using System.Diagnostics;
using PicPay.Worker.Extensions;

namespace PicPay.Worker.Events;

public class DomainEventsProcessor(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
{
    public async Task Run()
    {
        using var scope = serviceScopeFactory.CreateScope();

        await using var dataSource = NpgsqlDataSource.Create(configuration.Database().ConnectionString);
        await using var connection = await dataSource.OpenConnectionAsync();

        const string sql = @"
            UPDATE picpay.domain_events
            SET processor_id = @ProcessorId, status = 'Processing'
            WHERE id IN (
                SELECT id
                FROM picpay.domain_events
                WHERE processor_id IS NULL
                ORDER BY occurred_at
                LIMIT 100
            );

            SELECT id, type, data
            FROM picpay.domain_events
            WHERE processor_id = @ProcessorId AND processed_at IS NULL
            ORDER BY occurred_at;
        ";

        var events = await connection.QueryAsync<DomainEvent>(sql, new { ProcessorId = Guid.NewGuid() });

        var sw = Stopwatch.StartNew();

        foreach (var evt in events)
        {
            sw.Restart();

            dynamic data = GetData(evt);
            dynamic handler = GetHandler(scope, evt);
            string? error = null;

            try
            {
                await handler.Handle(evt.Id, data);
            }
            catch (Exception ex)
            {
                error = ex.Message + ex.InnerException?.Message;
            }

            const string update = @"
                UPDATE picpay.domain_events
                SET processed_at = now(), status = @Status, error = @Error, duration = @Duration
                WHERE id = @Id
            ";

            var parameters = new
            {
                evt.Id,
                error,
                Duration = sw.Elapsed.TotalMilliseconds,
                Status = error.HasValue() ? DomainEventStatus.Error.ToString() : DomainEventStatus.Success.ToString(),
            };
            sw.Stop();
            await connection.ExecuteAsync(update, parameters);
        }

        if (events.Count() == 100)
        {
            BackgroundJob.Enqueue<DomainEventsProcessor>(x => x.Run());
        }
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
}
