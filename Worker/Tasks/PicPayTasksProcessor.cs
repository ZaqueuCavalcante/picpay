using Dapper;
using Npgsql;
using Hangfire;
using PicPay.Shared;
using Newtonsoft.Json;
using PicPay.Api.Tasks;
using System.Diagnostics;
using PicPay.Worker.Extensions;

namespace PicPay.Worker.Tasks;

public class PicPayTasksProcessor(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
{
    public async Task Run()
    {
        using var scope = serviceScopeFactory.CreateScope();

        await using var dataSource = NpgsqlDataSource.Create(configuration.Database().ConnectionString);
        await using var connection = await dataSource.OpenConnectionAsync();

        const string sql = @"
            UPDATE picpay.tasks
            SET processor_id = @ProcessorId, status = 'Processing'
            WHERE id IN (
                SELECT id
                FROM picpay.tasks
                WHERE processor_id IS NULL
                ORDER BY created_at
                LIMIT 100
            );

            SELECT id, type, data
            FROM picpay.tasks
            WHERE processor_id = @ProcessorId AND processed_at IS NULL
            ORDER BY created_at;
        ";

        var tasks = await connection.QueryAsync<PicPayTask>(sql, new { ProcessorId = Guid.NewGuid() });

        var sw = Stopwatch.StartNew();

        foreach (var task in tasks)
        {
            sw.Restart();

            dynamic data = GetData(task);
            dynamic handler = GetHandler(scope, task);
            string? error = null;

            try
            {
                await handler.Handle(data);
            }
            catch (Exception ex)
            {
                error = ex.Message + ex.InnerException?.Message;
            }

            const string update = @"
                UPDATE picpay.tasks
                SET processed_at = now(), status = @Status, error = @Error, duration = @Duration
                WHERE id = @Id
            ";

            var parameters = new
            {
                task.Id,
                error,
                Duration = sw.Elapsed.TotalMilliseconds,
                Status = error.HasValue() ? PicPayTaskStatus.Error.ToString() : PicPayTaskStatus.Success.ToString(),
            };
            sw.Stop();

            await connection.ExecuteAsync(update, parameters);
        }

        if (tasks.Count() == 100)
        {
            BackgroundJob.Enqueue<PicPayTasksProcessor>(x => x.Run());
        }
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
}
